using EduNova.Domain.Common;

namespace EduNova.Domain.Entities;

public sealed class DeviceToken : GuidKeyEntity
{
    public string UserId { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime? LastUsedAt { get; set; }
}
