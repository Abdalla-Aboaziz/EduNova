using MediatR;

namespace EduNova.Application.Features.Authentication.Queries;

public record VerifyOtpQuery(string PhoneNumber, string Code) : IRequest<bool>;
