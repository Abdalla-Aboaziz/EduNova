namespace EduNova.Application.Features.Catalog.Responses
{
    /// <summary>One card of the Instructors screen grid.</summary>
    public sealed record InstructorListItemResponse(
        Guid Id,
        string FullName,
        string AcademicTitle,
        string? ImageUrl,
        int SubjectsCount);
}
