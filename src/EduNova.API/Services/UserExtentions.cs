using System.Security.Claims;

namespace EduNova.Application.Common
{
    public static class UserExtentions
    {
        public static string? GetUserId(this ClaimsPrincipal user) => user.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
