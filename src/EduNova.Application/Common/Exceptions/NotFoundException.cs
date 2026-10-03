namespace EduNova.Application.Common.Exceptions;
 
public class NotFoundException : AppException
{
    private const string DefaultCode = "NOT_FOUND";
 
    public NotFoundException(string message)
        : base(404, DefaultCode, message)
    {
    }
 
    public NotFoundException(string entityName, object key)
        : base(404, DefaultCode, $"{entityName} with id '{key}' was not found.")
    {
    }
}