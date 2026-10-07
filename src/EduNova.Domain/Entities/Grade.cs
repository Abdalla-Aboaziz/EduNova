using EduNova.Domain.Common;

namespace EduNova.Domain.Entities
{
    /// <summary>
    /// One student's final score in a subject. The year/semester context comes
    /// from the subject itself; the letter and GPA are computed from
    /// Score/MaxScore — never stored.
    /// </summary>
    public sealed class Grade : GuidKeyEntity
    {
        // TODO(AUTH): becomes an FK to ApplicationUser once member 1's auth lands.
        public string StudentId { get; set; } = string.Empty;

        public Guid SubjectId { get; set; }
        public Guid? OfferId { get; set; }

        /// <summary>Achieved points (e.g. 24) — displayed as 24/30 per the Figma.</summary>
        public decimal Score { get; set; }

        /// <summary>Maximum points for the subject's grade (e.g. 30).</summary>
        public decimal MaxScore { get; set; } = 30m;

        /// <summary>The "Exam Time" column on the Grade Sheet screen.</summary>
        public DateTime? ExamAt { get; set; }

        public Subject Subject { get; set; } = null!;
        public Offer? Offer { get; set; }
    }
}
