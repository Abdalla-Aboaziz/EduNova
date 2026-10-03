using EduNova.Application.Common.Interfaces;
using EduNova.Application.Interfaces;
using EduNova.Infrastructure.Data;
using EduNova.Infrastructure.Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EduNova.Infrastructure;

/// <summary>
/// Registers Infrastructure layer services into the DI container.
/// Configures EF Core with SQL Server using the "EduNova" connection string.
/// Called from Program.cs to keep registration organized per layer.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register EF Core DbContext with SQL Server
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("EduNova"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        // Register the DbContext abstraction for the Application layer
        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());


        // Register additional infrastructure services here
        // Example: services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IFileService, FileService>();
        return services;
    }
}
