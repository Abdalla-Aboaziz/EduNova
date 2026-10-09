using EduNova.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduNova.Application.Contracts.Services
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(AppUser user);
    }
}
