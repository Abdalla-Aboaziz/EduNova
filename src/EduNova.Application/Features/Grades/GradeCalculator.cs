namespace EduNova.Application.Features.Grades
{
    /// <summary>
    /// Grade business rules in one place (decision D7). The scale below is the
    /// single source of truth — letter and grade point are derived from the
    /// same band, so they can never disagree. Grades are stored as raw
    /// score/max (e.g. 24/30); everything here works on the percent.
    /// Adjust the table only if the faculty's grading system changes.
    /// </summary>
    public static class GradeCalculator
    {
        // (MinPercent, Letter, GradePoint) — highest band first, F is the catch-all.
        private static readonly (decimal MinPercent, string Letter, decimal GradePoint)[] Scale =
        [
            (90m, "A+",  4.0m),
            (85m, "A", 3.7m),
            (80m, "B+", 3.3m),
            (75m, "B",  3.0m),
            (70m, "C+", 2.7m),
            (65m, "C",  2.3m),
            (60m, "D+",  2.0m),
            (50m, "D",  1.0m),
            (0m,  "F",  0.0m),
        ];

        /// <summary>Converts a raw score to its percent (e.g. 24/30 → 80), rounded to 2 decimals.</summary>
        public static decimal ToPercent(decimal score, decimal maxScore) =>
            maxScore <= 0
                ? 0m
                : Math.Round(score / maxScore * 100m, 2, MidpointRounding.AwayFromZero);

        public static string ToLetter(decimal percent) =>
            BandFor(percent).Letter;

        public static decimal ToGradePoint(decimal percent) =>
            BandFor(percent).GradePoint;

        /// <summary>
        /// Weighted GPA: Σ(gradePoint × creditHours) / Σ(creditHours), rounded to
        /// 2 decimals. Returns null when there are no courses (a student with no
        /// grades has no GPA — callers surface it as null, not zero).
        /// </summary>
        public static decimal? CalculateGpa(IEnumerable<(decimal GradePoint, int CreditHours)> courses)
        {
            var totalHours = courses.Sum(c => c.CreditHours);
            if (totalHours == 0)
                return null;

            var weightedSum = courses.Sum(c => c.GradePoint * c.CreditHours);
            return Math.Round(weightedSum / totalHours, 2, MidpointRounding.AwayFromZero);
        }

        private static (decimal MinPercent, string Letter, decimal GradePoint) BandFor(decimal percent) =>
            Scale.First(band => percent >= band.MinPercent);
    }
}
