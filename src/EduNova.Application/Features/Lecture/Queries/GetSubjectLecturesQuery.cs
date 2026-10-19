using EduNova.Application.Common.Results;
using EduNova.Application.Features.Lecture.Responses;
using MediatR;

namespace EduNova.Application.Features.Lecture.Queries
{
    public class GetSubjectLecturesQuery : IRequest<Result<List<LectureResponse>>>
    {
        public GetSubjectLecturesQuery(Guid subjectId)
        {
            SubjectId = subjectId;
        }

        public Guid SubjectId { get; set; }
    }
}
