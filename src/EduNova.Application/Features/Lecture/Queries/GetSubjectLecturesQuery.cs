using EduNova.Application.Features.Lecture.Responses;
using MediatR;

namespace EduNova.Application.Features.Lecture.Queries
{
    public class GetSubjectLecturesQuery : IRequest<List<LectureResponse>>
    {
        public GetSubjectLecturesQuery(Guid subjectId)
        {
            SubjectId = subjectId;
        }

        public Guid SubjectId { get; set; }
    }
}
