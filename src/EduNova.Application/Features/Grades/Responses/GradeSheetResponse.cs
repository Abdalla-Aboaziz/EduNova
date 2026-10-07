namespace EduNova.Application.Features.Grades.Responses
{
    /// <summary>One graded subject row — raw score kept, letter/point computed.</summary>
    public sealed record GradeSheetSubjectResponse(
        Guid SubjectId,
        string Name,
        int CreditHours,
        decimal Score,
        decimal MaxScore,
        string ScoreText,
        decimal Percent,
        string Letter,
        decimal GradePoint,
        string? InstructorName,
        DateTime? ExamAt);

    /// <summary>One term block of the sheet with its own weighted GPA.</summary>
    public sealed record GradeSheetTermResponse(
        Guid YearId,
        string YearName,
        Guid SemesterId,
        string SemesterName,
        decimal? TermGpa,
        IReadOnlyList<GradeSheetSubjectResponse> Subjects);

    public sealed record GradeSheetStudentResponse(string FirstName, string FullName, decimal? OverallGpa);

    /// <summary>
    /// The Grade Sheet screen: student snapshot plus terms ordered chronologically,
    /// each with its subjects and term GPA. Empty terms list = the student has no
    /// grades yet (not an error).
    /// </summary>
    public sealed record GradeSheetResponse(
        GradeSheetStudentResponse Student,
        IReadOnlyList<GradeSheetTermResponse> Terms);
}
