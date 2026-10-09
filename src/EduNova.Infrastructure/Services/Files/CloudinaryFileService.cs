using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EduNova.Application.Common.Interfaces;
using EduNova.Domain.Entities;
using EduNova.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EduNova.Infrastructure.Services.Files;

/// <summary>
/// Cloudinary-backed implementation of <see cref="IFileService"/>.
/// The Cloudinary public_id is stored in <see cref="UploadedFiles.StoredFileName"/>,
/// so no schema change is needed to switch providers. Activated when the
/// FileStorage:Provider setting is "Cloudinary" with valid credentials;
/// otherwise the local <see cref="FileService"/> stays in use.
/// </summary>
public sealed class CloudinaryFileService(
    Cloudinary cloudinary,
    HttpClient httpClient,
    ApplicationDbContext context,
    IConfiguration configuration,
    ILogger<CloudinaryFileService> logger) : IFileService
{
    private readonly string _cloudName = configuration["FileStorage:Cloudinary:CloudName"] ?? string.Empty;

    public async Task<Guid> UploadAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        var publicId = Guid.NewGuid().ToString("N");

        using var buffer = new MemoryStream();
        await file.CopyToAsync(buffer, cancellationToken);
        buffer.Position = 0;

        UploadResult result = file.ContentType.StartsWith("video/", StringComparison.OrdinalIgnoreCase)
            ? await cloudinary.UploadAsync(new VideoUploadParams
            {
                File = new FileDescription(file.FileName, buffer),
                PublicId = publicId
            })
            : file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)
            ? await cloudinary.UploadAsync(new ImageUploadParams
            {
                File = new FileDescription(file.FileName, buffer),
                PublicId = publicId
            })
            : await cloudinary.UploadAsync(new RawUploadParams
            {
                File = new FileDescription(file.FileName, buffer),
                PublicId = publicId
            });

        if (result.Error is not null)
        {
            logger.LogError("Cloudinary upload failed: {Message}", result.Error.Message);
            throw new InvalidOperationException($"File upload failed: {result.Error.Message}");
        }

        var uploadedFile = new UploadedFiles
        {
            FileName = file.FileName,
            ContentType = file.ContentType,
            StoredFileName = result.PublicId,
            FileExtension = Path.GetExtension(file.FileName),
            FileSize = file.Length,
            UploadedAt = DateTime.UtcNow
        };

        await context.AddAsync(uploadedFile, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return uploadedFile.Id;
    }

    public async Task<(byte[] fileContent, string contentType, string fileName)> DownloadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var file = await context.Files.FindAsync(id, cancellationToken);
        if (file is null)
            return ([], string.Empty, string.Empty);

        var bytes = await httpClient.GetByteArrayAsync(BuildDeliveryUrl(file), cancellationToken);
        return (bytes, file.ContentType, file.FileName);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var file = await context.Files.FindAsync(id, cancellationToken);
            if (file is null)
                return false;

            await cloudinary.DestroyAsync(new DeletionParams(file.StoredFileName)
            {
                ResourceType = ResolveResourceType(file.ContentType)
            });

            context.Files.Remove(file);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete file with Id {FileId} from Cloudinary", id);
            return false;
        }
    }

    public async Task<(Stream? stream, string contentType, string fileName)> StreamAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var file = await context.Files.FindAsync(id, cancellationToken);
        if (file is null)
            return (null, string.Empty, string.Empty);

        var buffer = new MemoryStream();
        using var remote = await httpClient.GetStreamAsync(BuildDeliveryUrl(file), cancellationToken);
        await remote.CopyToAsync(buffer, cancellationToken);
        buffer.Position = 0;

        return (buffer, file.ContentType, file.FileName);
    }

    private string BuildDeliveryUrl(UploadedFiles file)
    {
        var resource = file.ContentType.StartsWith("video/", StringComparison.OrdinalIgnoreCase) ? "video"
            : file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase) ? "image"
            : "raw";

        return $"https://res.cloudinary.com/{_cloudName}/{resource}/upload/{file.StoredFileName}{file.FileExtension}";
    }

    private static ResourceType ResolveResourceType(string contentType)
        => contentType.StartsWith("video/", StringComparison.OrdinalIgnoreCase) ? ResourceType.Video
            : contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase) ? ResourceType.Image
            : ResourceType.Raw;
}