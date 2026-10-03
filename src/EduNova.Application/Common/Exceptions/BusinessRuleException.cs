namespace EduNova.Application.Common.Exceptions;
 

/// Thrown when a business rule is violated 

public class BusinessRuleException(string code, string message) 
    : AppException(422, code, message);