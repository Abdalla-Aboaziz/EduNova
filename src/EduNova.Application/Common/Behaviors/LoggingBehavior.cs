using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace EduNova.Application.Common.Behaviors;

/// <summary>
/// MediatR pipeline behavior that logs the start, end, and duration of each request.
/// Logs a warning if the request takes longer than 500ms.
/// </summary>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("EduNova Request: {Name} {@Request}", requestName, request);

        var stopwatch = Stopwatch.StartNew();

        var response = await next(cancellationToken);

        stopwatch.Stop();

        if (stopwatch.ElapsedMilliseconds > 500)
        {
            _logger.LogWarning(
                "EduNova Long Running Request: {Name} ({ElapsedMilliseconds}ms) {@Request}",
                requestName, stopwatch.ElapsedMilliseconds, request);
        }

        _logger.LogInformation(
            "EduNova Request Completed: {Name} in {ElapsedMilliseconds}ms",
            requestName, stopwatch.ElapsedMilliseconds);

        return response;
    }
}
