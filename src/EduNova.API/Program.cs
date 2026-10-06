using EduNova.API;
using EduNova.Application;
using EduNova.Domain;
using EduNova.Infrastructure;
using EduNova.Infrastructure.Data;
using EduNova.Infrastructure.Data.Seed;
using Serilog;

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
        .AddApiServices();

    var app = builder.Build();

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