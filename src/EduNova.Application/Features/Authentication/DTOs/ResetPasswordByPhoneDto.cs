namespace EduNova.Application.Features.Authentication.DTOs;

public class ResetPasswordByPhoneDto
{
    public string PhoneNumber { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}
