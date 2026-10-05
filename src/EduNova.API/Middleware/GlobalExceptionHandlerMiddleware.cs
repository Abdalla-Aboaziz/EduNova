using EduNova.API.Common;
using EduNova.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace EduNova.API.Middleware;



/// Catches unhandled exceptions and returns structured ProblemDetails JSON responses

public class GlobalExceptionHandlerMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionHandlerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            // The client closed the request; nothing to return and not an error
            logger.LogInformation("Request was cancelled by the client.");
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        // If the response already started, we can't change status code or body
        if (context.Response.HasStarted)
        {
            logger.LogError(ex, "Exception occurred after the response had started.");
            return;
        }

        ProblemDetails problem;

        if (ex is AppException appEx)
        {
            // Expected exception -> not a system error
            logger.LogWarning("Handled {AppExCode} ({AppExStatusCode}): {AppExMessage}",
                               appEx.Code, appEx.StatusCode, appEx.Message);

            problem = ApiProblem.FromAppException(appEx, context);
        }
        else
        {
            // Unexpected exception -> log everything, hide details from client
            logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);

            problem = ApiProblem.Unexpected(context);
        }

        context.Response.StatusCode = problem.Status!.Value;
        await context.Response.WriteAsJsonAsync(
            problem,
            options: null,
            contentType: "application/problem+json");
    }
}