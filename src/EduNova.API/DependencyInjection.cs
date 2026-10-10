using EduNova.API.Hubs;
using EduNova.API.Middleware;
using EduNova.API.Services;
using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Events.Jobs;
using Hangfire;
using Hangfire.SqlServer;
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
        // Register Swagger/OpenAPI (Swashbuckle)
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        // Also keep AddOpenApi for Microsoft OpenAPI support if available
        try { services.AddOpenApi(); } catch { }
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<Application.Contracts.Services.IMeetingLiveNotifier, MeetingLiveNotifier>();

        services.AddSignalR()
            .AddStackExchangeRedis(configuration.GetConnectionString("Redis")!);

        services.AddSignalRJwtSupport();

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("EduNova"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

        services.AddHangfireServer();
        services.AddScoped<ReminderDispatchJob>();

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

            app.UseHangfireDashboard("/hangfire");
        }
        catch
        {
            // If OpenAPI/Swagger services are not available for any reason, do not crash the app.
            // This keeps behavior safe in environments where Swagger is intentionally removed.
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapHub<MeetingHub>("/hubs/meetings");

        return app;
    }
}
