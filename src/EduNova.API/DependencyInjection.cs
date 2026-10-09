using EduNova.API.Hubs;
using EduNova.API.Middleware;
using EduNova.API.Services;
using EduNova.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace EduNova.API;

/// <summary>
/// Registers API layer services into the DI container.
/// Includes controllers, Swagger/OpenAPI, HttpContextAccessor, and CurrentUserService.
/// Called from Program.cs to keep registration organized per layer.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddOpenApi();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddSignalR()
            .AddStackExchangeRedis(configuration.GetConnectionString("Redis")!);

        services.AddSignalRJwtSupport();

        return services;
    }

    private static IServiceCollection AddSignalRJwtSupport(this IServiceCollection services)
    {
        services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            var events = options.Events ??= new JwtBearerEvents();
            var existingOnMessageReceived = events.OnMessageReceived;

            events.OnMessageReceived = async context =>
            {
                if (existingOnMessageReceived is not null)
                {
                    await existingOnMessageReceived(context);
                }

                var accessToken = context.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(accessToken) &&
                    context.HttpContext.Request.Path.StartsWithSegments("/hubs"))
                {
                    context.Token = accessToken;
                }
            };
        });

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

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "My API v1");
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.MapHub<MeetingHub>("/hubs/meetings");

        return app;
    }
}
