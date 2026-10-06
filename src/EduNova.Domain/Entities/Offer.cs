using EduNova.Domain.Common;

namespace EduNova.Domain.Entities
{
    /// <summary>
    /// A subject taught by an instructor in a specific semester. Carries its
    /// own SemesterId so a subject can be re-offered in later terms (e.g.
    /// retakes), possibly by a different instructor.
    /// </summary>
    public sealed class Offer : GuidKeyEntity
    {
        public Guid SubjectId { get; set; }
        public Guid InstructorId { get; set; }
        public Guid SemesterId { get; set; }

        public Subject Subject { get; set; } = null!;
        public Instructor Instructor { get; set; } = null!;
        public Semester Semester { get; set; } = null!;
    }
}
