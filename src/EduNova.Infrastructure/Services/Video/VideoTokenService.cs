using EduNova.Application.Contracts.Services;
using EduNova.Application.Features.Meetings.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EduNova.Infrastructure.Services.Video;

public sealed class VideoTokenService(IConfiguration configuration) : IVideoTokenService
{
    public Task<MeetingTokenResponse> GenerateAsync(Guid meetingId, string room, string userId, CancellationToken cancellationToken = default)
    {
        var provider = configuration["Video:Provider"] ?? "Custom";

        var ttlMinutes = int.TryParse(configuration["Video:TokenTtlMinutes"], out var parsed) ? parsed : 60;

        var signingKey = configuration["Video:SigningKey"]
            ?? configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Video signing key is not configured.");

        var expiresAt = DateTime.UtcNow.AddMinutes(ttlMinutes);

        var tokenHandler = new JwtSecurityTokenHandler();
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("room", room),
                new Claim("meeting_id", meetingId.ToString())
            }),
            Expires = expiresAt,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                SecurityAlgorithms.HmacSha256Signature)
        };

        return Task.FromResult(new MeetingTokenResponse
        {
            Token = tokenHandler.WriteToken(tokenHandler.CreateToken(descriptor)),
            Room = room,
            Provider = provider,
            ExpiresAt = expiresAt
        });
    }
}