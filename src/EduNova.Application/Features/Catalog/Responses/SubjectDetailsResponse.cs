namespace EduNova.Application.Features.Catalog.Responses
{
    /// <summary>Instructor card shown on the My Subject details screen.</summary>
    public sealed record SubjectInstructorResponse(
        Guid Id,
        string FullName,
        string AcademicTitle,
        string? ImageUrl);

    /// <summary>
    /// Full details for the My Subject screen. The lecture list itself comes
    /// from the Lectures module (GET /api/Lectures/subject/{subjectId}) —
    /// this response only carries the count (decision D6).
    /// </summary>
    public sealed record SubjectDetailsResponse(
        Guid Id,
        string Code,
        string Name,
        string? Description,
        int CreditHours,
        SubjectYearResponse Year,
        SubjectSemesterResponse Semester,
        IReadOnlyList<SubjectInstructorResponse> Instructors,
        int LecturesCount);
}
