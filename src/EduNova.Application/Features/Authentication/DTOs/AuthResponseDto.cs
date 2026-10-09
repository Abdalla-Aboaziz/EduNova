using System;
using System.Collections.Generic;
using System.Text;

namespace EduNova.Application.Features.Authentication.DTOs
{
    public class AuthResponseDto
    {
        public string DisplayName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
    }
}
