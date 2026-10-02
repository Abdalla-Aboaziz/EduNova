using EduNova.Application.Common.Interfaces;

namespace EduNova.API.Services;

/// <summary>
/// Implementation of ICurrentUserService that reads the current user
/// from the HttpContext. Useful for audit trails and authorization.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId =>
        _httpContextAccessor.HttpContext?.User?.FindFirst(
            System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
}
