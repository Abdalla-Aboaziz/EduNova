using EduNova.API;
using EduNova.Application;
using EduNova.Domain;
using EduNova.Infrastructure;
using Serilog;

// ── Bootstrap Serilog ───────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting EduNova API...");

    var builder = WebApplication.CreateBuilder(args);

    // ── Serilog ─────────────────────────────────────────────────────────
    builder.Host.UseSerilog((context, services, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));

    // ── Register services per layer (Clean Architecture DI) ─────────────
    builder.Services
        .AddDomainServices()
        .AddApplicationServices()
        .AddInfrastructureServices(builder.Configuration)
        .AddApiServices();

    var app = builder.Build();

    // ── Serilog request logging ─────────────────────────────────────────
    app.UseSerilogRequestLogging();

    // ── Configure middleware pipeline ───────────────────────────────────
    app.UseApiMiddleware();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
