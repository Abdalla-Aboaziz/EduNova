using Xunit;
using EduNova.Application.Common.Pagination;
using EduNova.Domain.Entities;
using EduNova.UnitTests.Testing;
using Microsoft.EntityFrameworkCore;

namespace EduNova.UnitTests.Pagination;

/// <summary>
/// Requirement checks 4-8: paging math on a real EF provider (InMemory),
/// TotalCount BEFORE pagination, TotalPages, empty sets and past-end pages.
/// </summary>
public class ToPagedResultAsyncTests
{
    private static List<Subject> BuildSubjects(int count, int creditHours = 3) =>
        Enumerable.Range(1, count)
            .Select(i => new Subject
            {
                Code = $"C{i:D4}",
                Name = $"Subject {i:D4}",
                CreditHours = creditHours,
                YearId = Guid.NewGuid(),
                SemesterId = Guid.NewGuid()
            })
            .ToList();

    [Fact]
    public async Task Returns_requested_page_with_metadata_from_the_requirement_example()
    {
        // 157 matching records, page = 2, pageSize = 10
        // → TotalCount 157, TotalPages 16, Items = rows 11-20.
        await using var db = TestDb.Create();
        db.Subjects.AddRange(BuildSubjects(157));
        await db.SaveChangesAsync();

        var page = await db.Subjects
            .AsNoTracking()
            .OrderBy(s => s.Name)
            .ToPagedResultAsync(new PagedRequest { Page = 2, PageSize = 10 });

        Assert.Equal(157, page.TotalCount);
        Assert.Equal(2, page.Page);
        Assert.Equal(10, page.PageSize);
        Assert.Equal(16, page.TotalPages);
        Assert.Equal(10, page.Items.Count);
        Assert.Equal("Subject 0011", page.Items[0].Name);
        Assert.Equal("Subject 0020", page.Items[^1].Name);
        Assert.True(page.HasNext);
        Assert.True(page.HasPrevious);
    }

    [Fact]
    public async Task TotalCount_counts_filtered_set_before_pagination()
    {
        // 25 subjects, 15 of them match the filter → page 2 of size 10 holds rows 11-15.
        await using var db = TestDb.Create();
        db.Subjects.AddRange(BuildSubjects(15, creditHours: 3));
        db.Subjects.AddRange(BuildSubjects(10, creditHours: 2));
        await db.SaveChangesAsync();

        var page = await db.Subjects
            .AsNoTracking()
            .Where(s => s.CreditHours == 3)
            .OrderBy(s => s.Name)
            .ToPagedResultAsync(new PagedRequest { Page = 2, PageSize = 10 });

        Assert.Equal(15, page.TotalCount);
        Assert.Equal(2, page.TotalPages);
        Assert.Equal(5, page.Items.Count);
        Assert.Equal("Subject 0011", page.Items[0].Name);
        Assert.Equal("Subject 0015", page.Items[^1].Name);
    }

    [Fact]
    public async Task Last_full_page_reports_no_next()
    {
        await using var db = TestDb.Create();
        db.Subjects.AddRange(BuildSubjects(25));
        await db.SaveChangesAsync();

        var page = await db.Subjects
            .AsNoTracking()
            .OrderBy(s => s.Name)
            .ToPagedResultAsync(new PagedRequest { Page = 3, PageSize = 10 });

        Assert.Equal(25, page.TotalCount);
        Assert.Equal(3, page.TotalPages);
        Assert.Equal(5, page.Items.Count);
        Assert.False(page.HasNext);
        Assert.True(page.HasPrevious);
    }

    [Fact]
    public async Task Page_beyond_last_returns_empty_items_with_correct_metadata()
    {
        await using var db = TestDb.Create();
        db.Subjects.AddRange(BuildSubjects(25));
        await db.SaveChangesAsync();

        var page = await db.Subjects
            .AsNoTracking()
            .OrderBy(s => s.Name)
            .ToPagedResultAsync(new PagedRequest { Page = 9, PageSize = 10 });

        Assert.Empty(page.Items);
        Assert.Equal(25, page.TotalCount);
        Assert.Equal(3, page.TotalPages);
        Assert.False(page.HasNext);
        Assert.True(page.HasPrevious);
    }

    [Fact]
    public async Task Empty_source_returns_empty_page_with_zero_total_pages()
    {
        await using var db = TestDb.Create();

        var page = await db.Subjects
            .AsNoTracking()
            .OrderBy(s => s.Name)
            .ToPagedResultAsync(new PagedRequest { Page = 1, PageSize = 10 });

        Assert.Empty(page.Items);
        Assert.Equal(0, page.TotalCount);
        Assert.Equal(0, page.TotalPages);
        Assert.False(page.HasNext);
        Assert.False(page.HasPrevious);
    }
}
