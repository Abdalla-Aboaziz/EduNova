using EduNova.Application.Common.Exceptions;
using EduNova.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace EduNova.API.Common;

/// <summary>
/// Single place that builds error ProblemDetails payloads. Both the global
/// exception middleware (exception path) and Result.ToActionResult (Result path)
/// go through it, so the two can never drift apart in wire format.
/// </summary>
public static class ApiProblem
{
    public static string TitleFor(int statusCode) => statusCode switch
    {
        400 => "Bad Request",
        401 => "Unauthorized",
        403 => "Forbidden",
        404 => "Not Found",
        409 => "Conflict",
        422 => "Unprocessable Entity",
        _ => "Server Error"
    };

    /// <summary>HTTP status for each Result failure category. Validation is 400 to match
    /// the existing ValidationException (AppException(400, "VALIDATION_ERROR", ...)).</summary>
    public static int StatusCodeFor(ErrorType type) => type switch
    {
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        ErrorType.Forbidden => StatusCodes.Status403Forbidden,
        _ => StatusCodes.Status500InternalServerError
    };

    /// <summary>Builds the payload for an expected application exception.</summary>
    public static ProblemDetails FromAppException(AppException ex, HttpContext context)
    {
        var problem = new ProblemDetails
        {
            Status = ex.StatusCode,
            Title = TitleFor(ex.StatusCode),
            Detail = ex.Message,
            Extensions =
            {
                ["code"] = ex.Code
            }
        };

        if (ex is ValidationException validationEx)
        {
            problem.Extensions["errors"] = validationEx.Errors;
        }

        return FillRequestInfo(problem, context);
    }

    /// <summary>Builds the payload for a failed Result.</summary>
    public static ProblemDetails FromError(Error error, HttpContext context)
    {
        var statusCode = StatusCodeFor(error.Type);
        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = TitleFor(statusCode),
            Detail = error.Message,
            Extensions =
            {
                ["code"] = error.Code
            }
        };

        if (error.Type == ErrorType.Validation && error.Details is not null)
        {
            problem.Extensions["errors"] = error.Details;
        }

        return FillRequestInfo(problem, context);
    }

    /// <summary>Builds the payload for an unexpected (unhandled) exception.</summary>
    public static ProblemDetails Unexpected(HttpContext context)
    {
        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = TitleFor(StatusCodes.Status500InternalServerError),
            Detail = "An unexpected error occurred. Please try again later.",
            Extensions =
            {
                ["code"] = "INTERNAL_ERROR"
            }
        };

        return FillRequestInfo(problem, context);
    }

    private static ProblemDetails FillRequestInfo(ProblemDetails problem, HttpContext context)
    {
        problem.Instance = context.Request.Path;
        problem.Extensions["traceId"] = context.TraceIdentifier;
        return problem;
    }
}
