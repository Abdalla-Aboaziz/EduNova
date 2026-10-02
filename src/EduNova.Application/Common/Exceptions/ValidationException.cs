namespace EduNova.Application.Common.Exceptions;

public class ValidationException() :
    AppException(400, DefaultCode, "One or more validation errors occurred.")
{
    private const string DefaultCode = "VALIDATION_ERROR";

    // Field name -> list of error messages
    public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();

    public ValidationException(IDictionary<string, string[]> errors)
        : this()
    {
        Errors = errors;
    }

    public ValidationException(string field, string message)
        : this(new Dictionary<string, string[]> { [field] = [message] })
    {
    }
}