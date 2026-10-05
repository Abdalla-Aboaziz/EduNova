using EduNova.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace EduNova.API.Common;

/// <summary>
/// Converts Result outcomes to IActionResult. Failure payloads go through
/// ApiProblem, so they are byte-for-byte the same shape the global exception
/// middleware returns for the equivalent AppException.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Success → 200 OK with the carried value.
    /// Failure → ProblemDetails (400/403/404/409/500 depending on the ErrorType).
    /// </summary>
    public static IActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller)
        => result.IsSuccess
            ? controller.Ok(result.Value)
            : ToErrorResult(result.Error, controller.HttpContext);

    /// <summary>
    /// Success → 204 No Content (for commands that return nothing).
    /// Failure → ProblemDetails, same mapping as above.
    /// </summary>
    public static IActionResult ToActionResult(this Result result, ControllerBase controller)
        => result.IsSuccess
            ? controller.NoContent()
            : ToErrorResult(result.Error, controller.HttpContext);

    private static IActionResult ToErrorResult(Error error, HttpContext context)
    {
        var problem = ApiProblem.FromError(error, context);

        return new ObjectResult(problem)
        {
            StatusCode = problem.Status,
            ContentTypes = { "application/problem+json" }
        };
    }
}
