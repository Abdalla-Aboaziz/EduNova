using EduNova.API;
using EduNova.Application;
using EduNova.Application.Features.Events.Jobs;
using EduNova.Domain;
using EduNova.Domain.Entities;
using EduNova.Infrastructure;
using EduNova.Infrastructure.Data;
using EduNova.Infrastructure.Data.Seed;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

// ── Bootstrap Serilog ───────────────────────────────────────────────────
// Temporary logger used only until the host is built and the real
// configuration is loaded. It captures errors that happen during startup.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting EduNova API...");

    var builder = WebApplication.CreateBuilder(args);

    // ── Serilog ─────────────────────────────────────────────────────────
    // Replaces the default logger. Levels, sinks and enrichers are read from
    // the "Serilog" section in appsettings.json.
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    // ── Register services per layer (Clean Architecture DI) ─────────────
    builder.Services
        .AddDomainServices()
        .AddApplicationServices()
        .AddInfrastructureServices(builder.Configuration)
        .AddApiServices(builder.Configuration);

    builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


    var jwtKey = builder.Configuration["Jwt:Key"]
        ?? throw new InvalidOperationException(
            "JWT signing key is not configured.");

    var jwtIssuer = builder.Configuration["Jwt:Issuer"]
        ?? throw new InvalidOperationException(
            "JWT issuer is not configured.");

    var jwtAudience = builder.Configuration["Jwt:Audience"]
        ?? throw new InvalidOperationException(
            "JWT audience is not configured.");

    builder.Services
        .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme =
                JwtBearerDefaults.AuthenticationScheme;

            options.DefaultChallengeScheme =
                JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtIssuer,

                    ValidateAudience = true,
                    ValidAudience = jwtAudience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)),

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
        });

    builder.Services.AddAuthorization();


    var app = builder.Build();

    // ── Recurring jobs ──────────────────────────────────────────────────
    // Every minute, dispatch reminders that are due and send them as push
    // notifications, then mark them as sent. Use the service-based API
    // (IRecurringJobManager) so Hangfire resolves its storage from DI
    // instead of relying on the static JobStorage.Current.
    using (var jobScope = app.Services.CreateScope())
    {
        var recurringJobManager = jobScope.ServiceProvider
            .GetRequiredService<IRecurringJobManager>();

        recurringJobManager.AddOrUpdate<ReminderDispatchJob>(
            "dispatch-due-reminders",
            job => job.RunAsync(CancellationToken.None),
            Cron.Minutely);
    }

    // ── Seed demo data ──────────────────────────────────────────────────
    // Development only: fills the catalog tables once and exits early when
    // data already exists (idempotent). Run migrations before starting.
    if (app.Environment.IsDevelopment())
    {
        using var seedScope = app.Services.CreateScope();
        await DbSeeder.SeedAsync(
            seedScope.ServiceProvider.GetRequiredService<ApplicationDbContext>(),
            seedScope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DbSeeder"));
    }

    // ── Serilog request logging ─────────────────────────────────────────
    // Writes one summary line per HTTP request (method, path, status, duration).
    // Placed before the API middleware so it records the final status code,
    // including the ones produced by the global exception handler.
    app.UseSerilogRequestLogging();

    // ── Configure middleware pipeline ───────────────────────────────────
    app.UseApiMiddleware();
    app.UseAuthentication();
    app.UseAuthorization();

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    // HostAbortedException is thrown on purpose by EF Core tooling
    // (e.g. dotnet ef migrations), so it must not be logged as a crash.
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    // Make sure all buffered logs are written before the process exits.
    Log.CloseAndFlush();
}