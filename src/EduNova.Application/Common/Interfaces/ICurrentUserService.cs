namespace EduNova.Application.Common.Interfaces;

/// <summary>
/// Provides access to the current authenticated user's information.
/// Implement this in the API/Infrastructure layer to read from HttpContext or claims.
/// </summary>
public interface ICurrentUserService
{
    string? UserId { get; }
}
