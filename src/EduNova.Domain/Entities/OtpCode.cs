using System;
using System.Collections.Generic;
using System.Text;

namespace EduNova.Domain.Entities
{
    public  class OtpCode
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; } = default!;
        public string CodeHash { get; set; } = default!;
        public DateTime ExpiresAtUtc { get; set; }
        public int Attempts { get; set; }
        public bool IsUsed { get; set; }
    }
}
