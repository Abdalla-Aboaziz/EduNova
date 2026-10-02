using EduNova.Application.Common.Interfaces;
using EduNova.API.Middleware;
using EduNova.API.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EduNova.API;

/// <summary>
/// Registers API layer services into the DI container.
/// Includes controllers, Swagger/OpenAPI, HttpContextAccessor, and CurrentUserService.
/// Called from Program.cs to keep registration organized per layer.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }

    /// <summary>
    /// Configures the API middleware pipeline.
    /// </summary>
    public static WebApplication UseApiMiddleware(this WebApplication app)
    {
        // Global exception handling — must be first in the pipeline
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}
