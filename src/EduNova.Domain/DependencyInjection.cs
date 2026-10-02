using Microsoft.Extensions.DependencyInjection;

namespace EduNova.Domain;

/// <summary>
/// Registers Domain layer services into the DI container.
/// Called from Program.cs to keep registration organized per layer.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        // Register domain-level services here as the project grows.
        // Example: services.AddScoped<IDomainService, DomainService>();

        return services;
    }
}
