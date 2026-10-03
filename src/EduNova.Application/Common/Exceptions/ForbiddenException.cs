namespace EduNova.Application.Common.Exceptions;

public class ForbiddenException(string 
    message = "You do not have permission to perform this action.")
    : AppException(403, DefaultCode, message)
{
    private const string DefaultCode = "FORBIDDEN";
}