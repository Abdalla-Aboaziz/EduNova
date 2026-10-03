using Microsoft.AspNetCore.Http;

namespace EduNova.Application.Common.Interfaces
{
    public interface IFileService
    {
        Task<Guid> UploadAsync(IFormFile file, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<(FileStream? stream, string contentType, string fileName)> StreamAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
