using EduNova.Application.Common.Interfaces;

namespace EduNova.UnitTests.Testing;

/// <summary>Test double for ICurrentUserService with a fixed identity (or none).</summary>
internal sealed class FakeCurrentUserService(string? userId) : ICurrentUserService
{
    public string? UserId { get; } = userId;
}
