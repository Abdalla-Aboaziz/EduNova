using EduNova.Application.Common.Caching;
using EduNova.Application.Common.Interfaces;
using EduNova.Application.Contracts;
using EduNova.Application.Contracts.Services;
using EduNova.Application.Interfaces;
using EduNova.Infrastructure.Data;
using EduNova.Infrastructure.Repositories;
using EduNova.Infrastructure.Services;
using EduNova.Infrastructure.Services.AccountSevice;
using EduNova.Infrastructure.Services.Files;
using EduNova.Infrastructure.Services.Notifications;
using EduNova.Infrastructure.Services.Video;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

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
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        // Register Redis
        var redisConnection = configuration.GetConnectionString("Redis")
            ?? throw new InvalidOperationException(
                "Redis connection string is not configured.");

        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(redisConnection));

        // Register the file storage provider. Cloudinary is used when the
        // FileStorage section selects it with valid credentials, otherwise
        // uploaded files stay on the local wwwroot disk (same as before).
        var fileStorageProvider = configuration["FileStorage:Provider"];
        var cloudName = configuration["FileStorage:Cloudinary:CloudName"];
        var apiKey = configuration["FileStorage:Cloudinary:ApiKey"];
        var apiSecret = configuration["FileStorage:Cloudinary:ApiSecret"];

        if (string.Equals(fileStorageProvider, "Cloudinary", StringComparison.OrdinalIgnoreCase)
            && !string.IsNullOrWhiteSpace(cloudName)
            && !string.IsNullOrWhiteSpace(apiKey)
            && !string.IsNullOrWhiteSpace(apiSecret))
        {
            services.AddSingleton(new CloudinaryDotNet.Cloudinary(
                new CloudinaryDotNet.Account(cloudName, apiKey, apiSecret)));
            services.AddHttpClient<Services.Files.CloudinaryFileService>();
            services.AddScoped<IFileService, Services.Files.CloudinaryFileService>();
        }
        else
        {
            services.AddScoped<IFileService, FileService>();
        }

        // register services in DI 
        services.AddScoped<ICacheRepository, CacheRepository>();
        services.AddScoped<ICacheService, CacheService>();

        // Register the push notification service. Firebase is used when a
        // service-account file is configured, otherwise a no-op implementation.
        var firebaseCredentialsPath = configuration["Firebase:CredentialsPath"];
        if (!string.IsNullOrWhiteSpace(firebaseCredentialsPath) && File.Exists(firebaseCredentialsPath))
        {
            try
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = CredentialFactory
                        .FromFile(firebaseCredentialsPath, "service_account")
                });
            }
            catch (InvalidOperationException)
            {
                // FirebaseApp is already initialized in this process.
            }

            services.AddScoped<IPushNotificationService, FirebasePushNotificationService>();
        }
        else
        {
            services.AddScoped<IPushNotificationService, NullPushNotificationService>();
        }

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IProfileService, Services.ProfileService.ProfileService>();
        services.AddScoped<IVideoTokenService, VideoTokenService>();
        return services;
    }
}
