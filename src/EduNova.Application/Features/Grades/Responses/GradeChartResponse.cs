namespace EduNova.Application.Features.Grades.Responses
{
    /// <summary>One x-point of the GPA trend chart, chronologically ordered.</summary>
    public sealed record GradeChartTermPoint(
        string Label,
        Guid YearId,
        Guid SemesterId,
        string YearName,
        string SemesterName,
        decimal? Gpa);

    /// <summary>How many of the student's grades fall in each letter band.</summary>
    public sealed record GradeChartLetterCount(string Letter, int Count);

    /// <summary>
    /// Data for the Grade Sheet chart: GPA per term (chronological trend) and
    /// the distribution of letter grades across all terms, ordered best-first.
    /// Empty arrays when the student has no grades yet.
    /// </summary>
    public sealed record GradeChartResponse(
        IReadOnlyList<GradeChartTermPoint> GpaTrend,
        IReadOnlyList<GradeChartLetterCount> GradeDistribution);
}
