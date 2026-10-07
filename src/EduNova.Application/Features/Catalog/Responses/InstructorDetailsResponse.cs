namespace EduNova.Application.Features.Catalog.Responses
{
    /// <summary>Year tab data for the Dr. Details screen (years the instructor teaches).</summary>
    public sealed record InstructorYearResponse(Guid Id, string Name);

    /// <summary>Subject card of the Dr. Details subjects grid.</summary>
    public sealed record InstructorSubjectResponse(Guid Id, string Name);

    /// <summary>
    /// Dr. Details screen: instructor info + the years they teach + their
    /// subjects, optionally narrowed by year and semester.
    /// </summary>
    public sealed record InstructorDetailsResponse(
        Guid Id,
        string FullName,
        string AcademicTitle,
        string? Bio,
        string? Email,
        string? ImageUrl,
        IReadOnlyList<InstructorYearResponse> YearsTeaching,
        IReadOnlyList<InstructorSubjectResponse> Subjects);
}
