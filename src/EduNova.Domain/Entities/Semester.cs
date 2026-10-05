using EduNova.Domain.Common;

namespace EduNova.Domain.Entities
{
    /// <summary>
    /// Term inside an academic year — "Semester 1" / "Semester 2"
    /// (two semesters per year, confirmed from the Figma filter screen).
    /// </summary>
    public sealed class Semester : GuidKeyEntity
    {
        public Guid YearId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Order { get; set; }

        public Year Year { get; set; } = null!;
    }
}
