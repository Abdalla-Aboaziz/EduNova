using EduNova.Application.Features.Grades;
using Xunit;

namespace EduNova.UnitTests.Grades;

/// <summary>
/// Grade scale edge cases (T4.2 DoD): band boundaries, the score/max to
/// percent conversion and weighted GPA math.
/// </summary>
public class GradeCalculatorTests
{
    [Theory]
    [InlineData(90, "A")]
    [InlineData(89.99, "A-")]
    [InlineData(85, "A-")]
    [InlineData(84.99, "B+")]
    [InlineData(80, "B+")]
    [InlineData(79.99, "B")]
    [InlineData(75, "B")]
    [InlineData(74.99, "C+")]
    [InlineData(70, "C+")]
    [InlineData(69.99, "C")]
    [InlineData(65, "C")]
    [InlineData(64.99, "D")]
    [InlineData(60, "D")]
    [InlineData(59.99, "E")]
    [InlineData(50, "E")]
    [InlineData(49.99, "F")]
    [InlineData(0, "F")]
    public void ToLetter_maps_band_boundaries_exactly(double percent, string expectedLetter)
    {
        Assert.Equal(expectedLetter, GradeCalculator.ToLetter((decimal)percent));
    }

    [Theory]
    [InlineData(92, "A", 4.0)]
    [InlineData(87, "A-", 3.7)]
    [InlineData(82, "B+", 3.3)]
    [InlineData(77, "B", 3.0)]
    [InlineData(72, "C+", 2.7)]
    [InlineData(67, "C", 2.3)]
    [InlineData(62, "D", 2.0)]
    [InlineData(55, "E", 1.0)]
    [InlineData(30, "F", 0.0)]
    public void Letter_and_grade_point_come_from_the_same_band(
        double percent, string expectedLetter, double expectedPoint)
    {
        Assert.Equal(expectedLetter, GradeCalculator.ToLetter((decimal)percent));
        Assert.Equal((decimal)expectedPoint, GradeCalculator.ToGradePoint((decimal)percent));
    }

    [Theory]
    [InlineData(24, 30, 80)]          // the Figma example: 24/30
    [InlineData(91.5, 100, 91.5)]
    [InlineData(47, 60, 78.33)]       // repeating decimal rounds to 2 places
    [InlineData(0, 30, 0)]
    [InlineData(35, 30, 116.67)]      // bonus marks — no clamping, still just a percent
    public void ToPercent_converts_score_over_max(double score, double max, double expected)
    {
        Assert.Equal((decimal)expected, GradeCalculator.ToPercent((decimal)score, (decimal)max));
    }

    [Fact]
    public void ToPercent_with_zero_max_returns_zero_instead_of_throwing()
    {
        Assert.Equal(0m, GradeCalculator.ToPercent(24m, 0m));
    }

    [Fact]
    public void Gpa_is_weighted_by_credit_hours()
    {
        var gpa = GradeCalculator.CalculateGpa(
        [
            (GradePoint: 4.0m, CreditHours: 4),   // A in a heavy course
            (GradePoint: 3.0m, CreditHours: 1),   // B in a light one
        ]);

        Assert.Equal(3.8m, gpa);   // (16 + 3) / 5
    }

    [Fact]
    public void Gpa_rounds_to_two_decimals()
    {
        var gpa = GradeCalculator.CalculateGpa(
        [
            (GradePoint: 4.0m, CreditHours: 1),
            (GradePoint: 3.7m, CreditHours: 1),
            (GradePoint: 3.3m, CreditHours: 1),
        ]);

        Assert.Equal(3.67m, gpa);   // 11 / 3 = 3.666…
    }

    [Fact]
    public void Gpa_treats_a_failed_course_as_zero_points_not_a_skip()
    {
        var gpa = GradeCalculator.CalculateGpa(
        [
            (GradePoint: 4.0m, CreditHours: 3),
            (GradePoint: 0.0m, CreditHours: 3),
        ]);

        Assert.Equal(2.0m, gpa);
    }

    [Fact]
    public void Gpa_is_null_when_there_are_no_courses()
    {
        Assert.Null(GradeCalculator.CalculateGpa([]));
    }
}
