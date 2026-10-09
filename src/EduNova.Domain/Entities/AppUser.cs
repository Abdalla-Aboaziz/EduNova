using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduNova.Domain.Entities
{
    public  class AppUser : IdentityUser<Guid>
    {
        public string DisplayName { get; set; } = null!;
    }
}
