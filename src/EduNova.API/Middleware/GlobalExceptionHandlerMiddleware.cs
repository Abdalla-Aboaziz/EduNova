using System.Net;
using System.Text.Json;
using EduNova.Application.Common.Exceptions;

namespace EduNova.API.Middleware;

/// <summary>
/// Global exception handler middleware.
/// Catches unhandled exceptions and returns structured JSON error responses.
/// </summary>
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, response) = exception switch
        {
            ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                new
                {
                    title = "Validation Error",
                    status = (int)HttpStatusCode.BadRequest,
                    errors = validationEx.Errors
                } as object
            ),
            NotFoundException => (
                HttpStatusCode.NotFound,
                new
                {
                    title = "Not Found",
                    status = (int)HttpStatusCode.NotFound,
                    detail = exception.Message
                } as object
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                new
                {
                    title = "Server Error",
                    status = (int)HttpStatusCode.InternalServerError,
                    detail = "An unexpected error occurred."
                } as object
            )
        };

        context.Response.StatusCode = (int)statusCode;

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
    }
}
