namespace EduNova.Application.Common.Exceptions;
 

/// Base class for all expected application exceptions.
/// The global exception handler maps StatusCode/Code to the HTTP response.

public abstract class AppException(int statusCode, string code, string message) 
    : Exception(message)
{
    public int StatusCode { get; } = statusCode;
    public string Code { get; } = code;
}