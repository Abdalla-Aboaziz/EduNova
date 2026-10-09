using Microsoft.AspNetCore.Http;

namespace EduNova.Application.Common.Interfaces
{
    public interface IFileService
    {
        Task<Guid> UploadAsync(IFormFile file, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<(Stream? stream, string contentType, string fileName)> StreamAsync(Guid id, CancellationToken cancellationToken = default);
        Task<(byte[] fileContent, string contentType, string fileName)> DownloadAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
