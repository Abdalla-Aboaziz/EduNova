namespace EduNova.Application.Common.Exceptions;

public class ConflictException : AppException
{
    private const string DefaultCode = "CONFLICT";

    public ConflictException(string message)
        : base(409, DefaultCode, message)
    {
    }

    public ConflictException(string code, string message)
        : base(409, code, message)
    {
    }
}