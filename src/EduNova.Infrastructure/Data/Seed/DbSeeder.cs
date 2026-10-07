using EduNova.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EduNova.Infrastructure.Data.Seed;

/// <summary>
/// Fills the demo data (catalog + grades) on first run (Development only).
/// Ids are deterministic so every team member gets the same rows. Idempotent
/// per block: the catalog seeds when Years is empty, grades seed when Grades
/// is empty — so databases seeded before the grades step pick them up on the
/// next run. Run migrations before starting the app.
/// </summary>
public static class DbSeeder
{
    // Deterministic id ranges (encoded in the last 12 hex digits):
    //   1-4    years        11-18  semesters
    //   21-40  subjects     41-48  instructors
    //   51-70  offers
    private static Guid Id(int n) => new($"00000000-0000-0000-0000-{n:D12}");

    public static async Task SeedAsync(ApplicationDbContext db, ILogger logger)
    {
        await SeedCatalogAsync(db, logger);
        await SeedGradesAsync(db, logger);
    }

    private static async Task SeedCatalogAsync(ApplicationDbContext db, ILogger logger)
    {
        if (await db.Years.AnyAsync())
        {
            logger.LogDebug("Seed skipped — catalog data already present.");
            return;
        }

        var years = new List<Year>
        {
            new() { Id = Id(1), Number = 1, Name = "First Year" },
            new() { Id = Id(2), Number = 2, Name = "Second Year" },
            new() { Id = Id(3), Number = 3, Name = "Third Year" },
            new() { Id = Id(4), Number = 4, Name = "Fourth Year" },
        };

        // Year 1 -> ids 11, 12 ... Year 4 -> ids 17, 18.
        var semesters = new List<Semester>();
        foreach (var year in years)
        {
            semesters.Add(new Semester
            {
                Id = Id(10 + year.Number * 2 - 1),
                YearId = year.Id,
                Name = "Semester 1",
                Order = 1
            });
            semesters.Add(new Semester
            {
                Id = Id(10 + year.Number * 2),
                YearId = year.Id,
                Name = "Semester 2",
                Order = 2
            });
        }

        // Index 0 is Dr. Reham Ahmed so English (first subject) matches the Figma screens.
        var instructors = new List<Instructor>
        {
            new() { Id = Id(41), FullName = "Dr. Reham Ahmed", AcademicTitle = "Professor", Email = "r.ahmed@edinova.edu",
                    Bio = "20 years of teaching experience with a focus on student-centered learning." },
            new() { Id = Id(42), FullName = "Dr. Ahmed Hassan", AcademicTitle = "Professor", Email = "a.hassan@edinova.edu",
                    Bio = "Researches curriculum design and assessment methods." },
            new() { Id = Id(43), FullName = "Dr. Ranny Ali", AcademicTitle = "Associate Professor", Email = "r.ali@edinova.edu",
                    Bio = "Leads hands-on labs and graduation project mentoring." },
            new() { Id = Id(44), FullName = "Dr. Al Mohamed", AcademicTitle = "Associate Professor", Email = "al.mohamed@edinova.edu",
                    Bio = "Interested in applied mathematics and modeling." },
            new() { Id = Id(45), FullName = "Dr. Amira Atta", AcademicTitle = "Lecturer", Email = "a.atta@edinova.edu",
                    Bio = "Teaches core science courses with interactive demos." },
            new() { Id = Id(46), FullName = "Dr. Ahmed Hosny", AcademicTitle = "Lecturer", Email = "a.hosny@edinova.edu",
                    Bio = "Focuses on systems fundamentals and practice sessions." },
            new() { Id = Id(47), FullName = "Dr. Sara Mahmoud", AcademicTitle = "Associate Professor", Email = "s.mahmoud@edinova.edu",
                    Bio = "Works on data systems and student research projects." },
            new() { Id = Id(48), FullName = "Dr. Omar Khaled", AcademicTitle = "Lecturer", Email = "o.khaled@edinova.edu",
                    Bio = "Teaches programming fundamentals and modern tooling." },
        };

        // Year 1 -> ids 21-25, Year 2 -> 26-30, Year 3 -> 31-34, Year 4 -> 35-36.
        var subjects = new List<Subject>
        {
            new() { Id = Id(21), Code = "ENG101", Name = "English", CreditHours = 2, YearId = Id(1), SemesterId = Id(11),
                    Description = "Language skills for academic and technical contexts." },
            new() { Id = Id(22), Code = "ARB101", Name = "Arabic", CreditHours = 2, YearId = Id(1), SemesterId = Id(11),
                    Description = "Grammar, composition and academic writing." },
            new() { Id = Id(23), Code = "MTH101", Name = "Math", CreditHours = 4, YearId = Id(1), SemesterId = Id(11),
                    Description = "Calculus and linear algebra foundations." },
            new() { Id = Id(24), Code = "PHY101", Name = "Physics", CreditHours = 3, YearId = Id(1), SemesterId = Id(12),
                    Description = "Mechanics, waves and thermodynamics." },
            new() { Id = Id(25), Code = "CHM101", Name = "Chemistry", CreditHours = 3, YearId = Id(1), SemesterId = Id(12),
                    Description = "General chemistry principles and lab work." },
            new() { Id = Id(26), Code = "DSA201", Name = "Data Structures", CreditHours = 3, YearId = Id(2), SemesterId = Id(13),
                    Description = "Lists, trees, graphs and complexity analysis." },
            new() { Id = Id(27), Code = "OOP201", Name = "Object Oriented Programming", CreditHours = 3, YearId = Id(2), SemesterId = Id(13),
                    Description = "Object-oriented design principles and patterns." },
            new() { Id = Id(28), Code = "STA201", Name = "Statistics", CreditHours = 3, YearId = Id(2), SemesterId = Id(13),
                    Description = "Descriptive and inferential statistics." },
            new() { Id = Id(29), Code = "DBI201", Name = "Databases", CreditHours = 3, YearId = Id(2), SemesterId = Id(14),
                    Description = "Relational modeling and SQL." },
            new() { Id = Id(30), Code = "TWC201", Name = "Technical Writing", CreditHours = 2, YearId = Id(2), SemesterId = Id(14),
                    Description = "Documentation and scientific writing skills." },
            new() { Id = Id(31), Code = "SWE301", Name = "Software Engineering", CreditHours = 3, YearId = Id(3), SemesterId = Id(15),
                    Description = "Processes, requirements and team development." },
            new() { Id = Id(32), Code = "OSY301", Name = "Operating Systems", CreditHours = 3, YearId = Id(3), SemesterId = Id(15),
                    Description = "Processes, memory and file systems." },
            new() { Id = Id(33), Code = "NET301", Name = "Computer Networks", CreditHours = 3, YearId = Id(3), SemesterId = Id(16),
                    Description = "TCP/IP stack and network programming." },
            new() { Id = Id(34), Code = "WEB301", Name = "Web Development", CreditHours = 3, YearId = Id(3), SemesterId = Id(16),
                    Description = "Modern front-end and back-end basics." },
            new() { Id = Id(35), Code = "AII401", Name = "Artificial Intelligence", CreditHours = 3, YearId = Id(4), SemesterId = Id(17),
                    Description = "Search, knowledge representation and learning basics." },
            new() { Id = Id(36), Code = "MLN401", Name = "Machine Learning", CreditHours = 3, YearId = Id(4), SemesterId = Id(17),
                    Description = "Supervised and unsupervised learning models." },
        };

        // One offer per subject, instructors round-robin, in the subject's own semester.
        var offers = new List<Offer>();
        for (var i = 0; i < subjects.Count; i++)
        {
            offers.Add(new Offer
            {
                Id = Id(51 + i),
                SubjectId = subjects[i].Id,
                InstructorId = instructors[i % instructors.Count].Id,
                SemesterId = subjects[i].SemesterId
            });
        }

        // Two subjects get a second instructor to exercise the multi-offer paths.
        offers.Add(new Offer { Id = Id(70), SubjectId = subjects[0].Id, InstructorId = Id(47), SemesterId = subjects[0].SemesterId });
        offers.Add(new Offer { Id = Id(71), SubjectId = subjects[14].Id, InstructorId = Id(42), SemesterId = subjects[14].SemesterId });

        db.Years.AddRange(years);
        db.Semesters.AddRange(semesters);
        db.Instructors.AddRange(instructors);
        db.Subjects.AddRange(subjects);
        db.Offers.AddRange(offers);
        await db.SaveChangesAsync();

        logger.LogInformation(
            "Seeded catalog demo data: {Years} years, {Semesters} semesters, {Subjects} subjects, {Instructors} instructors, {Offers} offers.",
            years.Count, semesters.Count, subjects.Count, instructors.Count, offers.Count);
    }

    /// <summary>
    /// Demo transcript for the demo student — MUST match EduNova:DemoStudentId
    /// in appsettings. Six graded terms (Y1S1..Y3S2) with a rising trend;
    /// the current term (Year 4 - Semester 1) stays without grades — the
    /// student is studying it now.
    /// </summary>
    private static async Task SeedGradesAsync(ApplicationDbContext db, ILogger logger)
    {
        if (await db.Grades.AnyAsync())
        {
            logger.LogDebug("Seed skipped — grade data already present.");
            return;
        }

        const string studentId = "demo-student-1";
        var grades = new List<Grade>
        {
            // Year 1 — Semester 1 (exams Jan 2024)
            G(studentId, 21, 51, 21m,   Exam(2024, 1, 20)),   // English  70% -> C+
            G(studentId, 22, 52, 22.5m, Exam(2024, 1, 20)),   // Arabic   75% -> B
            G(studentId, 23, 53, 25.5m, Exam(2024, 1, 20)),   // Math     85% -> A
            // Year 1 — Semester 2 (exams Jun 2024)
            G(studentId, 24, 54, 24m,   Exam(2024, 6, 15)),   // Physics   80% -> B+
            G(studentId, 25, 55, 24.9m, Exam(2024, 6, 15)),   // Chemistry 83% -> B+
            // Year 2 — Semester 1 (exams Jan 2025)
            G(studentId, 26, 56, 25.5m, Exam(2025, 1, 18)),   // Data Structures 85% -> A
            G(studentId, 27, 57, 25.5m, Exam(2025, 1, 18)),   // OOP             85% -> A
            G(studentId, 28, 58, 24m,   Exam(2025, 1, 18)),   // Statistics      80% -> B+
            // Year 2 — Semester 2 (exams Jun 2025)
            G(studentId, 29, 59, 27m,   Exam(2025, 6, 14)),   // Databases         90% -> A+
            G(studentId, 30, 60, 24.9m, Exam(2025, 6, 14)),   // Technical Writing 83% -> B+
            // Year 3 — Semester 1 (exams Jan 2026)
            G(studentId, 31, 61, 27m,   Exam(2026, 1, 17)),   // Software Engineering 90% -> A+
            G(studentId, 32, 62, 25.5m, Exam(2026, 1, 17)),   // Operating Systems    85% -> A
            // Year 3 — Semester 2 (exams Jun 2026)
            G(studentId, 33, 63, 27m,   Exam(2026, 6, 13)),   // Computer Networks 90% -> A+
            G(studentId, 34, 64, 26.1m, Exam(2026, 6, 13)),   // Web Development   87% -> A
        };

        db.Grades.AddRange(grades);
        await db.SaveChangesAsync();

        logger.LogInformation("Seeded {Count} demo grades for {Student}.", grades.Count, studentId);
    }

    private static Grade G(string studentId, int subjectNum, int offerNum, decimal score, DateTime examAt) =>
        new()
        {
            StudentId = studentId,
            SubjectId = Id(subjectNum),
            OfferId = Id(offerNum),
            Score = score,
            MaxScore = 30m,
            ExamAt = examAt
        };

    private static DateTime Exam(int year, int month, int day) =>
        new(year, month, day, 10, 0, 0, DateTimeKind.Utc);
}