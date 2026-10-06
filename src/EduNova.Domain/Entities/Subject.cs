using EduNova.Domain.Common;

namespace EduNova.Domain.Entities
{
    /// <summary>
    /// A course placed in one year and semester of the curriculum.
    /// Lectures (Lectures module) reference SubjectId by value — no FK here.
    /// </summary>
    public sealed class Subject : GuidKeyEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int CreditHours { get; set; }

        public Guid YearId { get; set; }
        public Guid SemesterId { get; set; }

        public Year Year { get; set; } = null!;
        public Semester Semester { get; set; } = null!;

        public ICollection<Offer> Offers { get; set; } = [];
    }
}
