namespace EduNova.Application.Features.Catalog.Responses
{
    public sealed record HomeYearResponse(Guid Id, string Name);

    public sealed record HomeSemesterResponse(Guid Id, string Name);

    /// <summary>
    /// Demo student until Auth lands — the id/name come from the EduNova
    /// config section (TODO(AUTH)) and the GPA is filled by the grades work.
    /// </summary>
    public sealed record HomeStudentResponse(string Id, string FirstName, string FullName, decimal? Gpa);

    /// <summary>One subject card of the Home screen's current-term grid.</summary>
    public sealed record HomeSubjectResponse(
        Guid Id,
        string Name,
        string? InstructorName,
        int CreditHours,
        int LecturesCount);

    /// <summary>
    /// Everything the Home screen needs in one request: current term context,
    /// the student snapshot and their current-term subjects. The quick-action
    /// cards (Events/Attendance/Notices/...) are static navigation — no API
    /// data needed for them; upcoming events join when the Events module lands.
    /// </summary>
    public sealed record HomeSummaryResponse(
        HomeYearResponse CurrentYear,
        HomeSemesterResponse CurrentSemester,
        HomeStudentResponse Student,
        IReadOnlyList<HomeSubjectResponse> MySubjects);
}
