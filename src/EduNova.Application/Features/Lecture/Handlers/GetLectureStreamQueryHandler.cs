using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Lecture.Queries;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Lecture.Handlers
{
    public class GetLectureStreamQueryHandler : IRequestHandler<GetLectureStreamQuery, (FileStream? stream, string ContentType, string FileName)>
    {
        private readonly IApplicationDbContext _context;
        private readonly IFileService _fileService;

        public GetLectureStreamQueryHandler(IApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        public async Task<(FileStream? stream, string ContentType, string FileName)> Handle(GetLectureStreamQuery request, CancellationToken cancellationToken)
        {
            var videoFileId = await _context.Lectures
             .AsNoTracking()
             .Where(l => l.Id == request.Id)
             .Select(l => (Guid?)l.VideoFileId)
             .FirstOrDefaultAsync(cancellationToken);

            if (videoFileId is null)
                return (null, string.Empty, string.Empty);

            return await _fileService.StreamAsync(videoFileId.Value, cancellationToken);
        }
    }
}
