using EduNova.Domain.Common;

namespace EduNova.Domain.Entities
{
    /// <summary>
    /// Academic year — "First Year" through "Fourth Year".
    /// </summary>
    public sealed class Year : GuidKeyEntity
    {
        public int Number { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<Semester> Semesters { get; set; } = [];
    }
}
