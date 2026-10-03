using EduNova.Application.Features.Lecture.Responses;
using MediatR;

namespace EduNova.Application.Features.Lecture.Queries
{
    public class GetMyListQuery : IRequest<List<MyListResponse>>
    {

    }
}
