using EduNova.API.Middleware;
using EduNova.API.Services;
using EduNova.Application.Common.Interfaces;

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
        // Register Swagger/OpenAPI (Swashbuckle)
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        // Also keep AddOpenApi for Microsoft OpenAPI support if available
        try { services.AddOpenApi(); } catch { }
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
        // Map OpenAPI document and enable Swagger UI. By default this was enabled only
        // in Development environment; enable unconditionally so Swagger is available
        // when running locally with other environment settings as well.
        try
        {
            app.MapOpenApi();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "My API v1");
            });
        }
        catch
        {
            // If OpenAPI/Swagger services are not available for any reason, do not crash the app.
            // This keeps behavior safe in environments where Swagger is intentionally removed.
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}
