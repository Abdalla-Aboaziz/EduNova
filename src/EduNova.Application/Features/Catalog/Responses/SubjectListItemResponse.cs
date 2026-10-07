namespace EduNova.Application.Features.Catalog.Responses
{
    public sealed record SubjectYearResponse(Guid Id, string Name);

    public sealed record SubjectSemesterResponse(Guid Id, string Name);

    /// <summary>One row of the Filter screen subjects list.</summary>
    public sealed record SubjectListItemResponse(
        Guid Id,
        string Code,
        string Name,
        int CreditHours,
        SubjectYearResponse Year,
        SubjectSemesterResponse Semester,
        IReadOnlyList<string> Instructors);
}
