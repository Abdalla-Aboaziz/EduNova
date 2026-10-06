using Microsoft.Extensions.DependencyInjection;

namespace EduNova.Domain;


public static class DependencyInjection
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        // Register domain-level services here as the project grows.
        // Example: services.AddScoped<IDomainService, DomainService>();

        return services;
    }
}
