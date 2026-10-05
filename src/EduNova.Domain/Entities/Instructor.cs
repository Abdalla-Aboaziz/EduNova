using EduNova.Domain.Common;

namespace EduNova.Domain.Entities
{
    /// <summary>
    /// Teaching staff member. Photos live on ImageUrl until the shared
    /// file storage service is available.
    /// </summary>
    public sealed class Instructor : GuidKeyEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string AcademicTitle { get; set; } = string.Empty;
        public string? Bio { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;

        public ICollection<Offer> Offers { get; set; } = [];
    }
}
