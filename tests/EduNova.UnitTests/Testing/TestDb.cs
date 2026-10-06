using EduNova.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduNova.UnitTests.Testing;

/// <summary>
/// Creates an ApplicationDbContext backed by a fresh EF InMemory database
/// per call so tests never share state.
/// </summary>
internal static class TestDb
{
    public static ApplicationDbContext Create() => new(
        new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"EduNova.Tests.{Guid.NewGuid()}")
            .Options);
}
