using EduNova.Application.Common.Results;
using EduNova.Application.Features.Profile.DTOs;
using MediatR;

namespace EduNova.Application.Features.Profile.Queries;

public class GetCurrentUserQuery : IRequest<Result<ProfileResponseDto>>
{
}