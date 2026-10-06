using Xunit;
using System.Linq.Expressions;
using EduNova.Application.Common.Specifications;
using EduNova.Application.Features.Catalog.Specifications;
using EduNova.Domain.Entities;

namespace EduNova.UnitTests.Specifications;

/// <summary>
/// Structural guarantee (requirement 10): specifications only DESCRIBE the
/// query — Skip/Take may exist only in QueryableExtensions.ToPagedResultAsync.
/// The test walks the produced expression tree and fails on any Skip/Take call.
/// </summary>
public class SpecificationNoSkipTakeTests
{
    [Fact]
    public void Base_specification_never_applies_skip_or_take()
    {
        Specification<Subject> spec = new EmptySubjectSpecification();

        AssertNoSkipOrTake(spec.ApplyTo(Enumerable.Empty<Subject>().AsQueryable()));
    }

    [Fact]
    public void Subject_search_specification_never_applies_skip_or_take()
    {
        var spec = new SubjectSearchSpecification("database", Guid.NewGuid(), Guid.NewGuid());

        AssertNoSkipOrTake(spec.ApplyTo(Enumerable.Empty<Subject>().AsQueryable()));
    }

    [Fact]
    public void Instructor_search_specification_never_applies_skip_or_take()
    {
        var spec = new InstructorSearchSpecification("ahmed");

        AssertNoSkipOrTake(spec.ApplyTo(Enumerable.Empty<Instructor>().AsQueryable()));
    }

    [Fact]
    public void Instructor_offers_specification_never_applies_skip_or_take()
    {
        var spec = new InstructorOffersSpecification(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        AssertNoSkipOrTake(spec.ApplyTo(Enumerable.Empty<Offer>().AsQueryable()));
    }

    [Fact]
    public void Subject_by_id_specification_never_applies_skip_or_take()
    {
        var spec = new SubjectByIdSpecification(Guid.NewGuid());

        AssertNoSkipOrTake(spec.ApplyTo(Enumerable.Empty<Subject>().AsQueryable()));
    }

    private static void AssertNoSkipOrTake<T>(IQueryable<T> query) =>
        Assert.False(
            ContainsSkipOrTake(query.Expression),
            "Specification output contains Skip/Take — pagination must stay in ToPagedResultAsync.");

    private static bool ContainsSkipOrTake(Expression? expression)
    {
        if (expression is null)
            return false;

        if (expression is MethodCallExpression call)
        {
            if (call.Method.DeclaringType == typeof(Queryable)
                && call.Method.Name is nameof(Queryable.Skip) or nameof(Queryable.Take))
                return true;

            return call.Arguments.Any(ContainsSkipOrTake)
                   || ContainsSkipOrTake(call.Object);
        }

        if (expression is UnaryExpression unary)
            return ContainsSkipOrTake(unary.Operand);

        if (expression is NewExpression newExpression)
            return newExpression.Arguments.Any(ContainsSkipOrTake);

        return false;
    }

    private sealed class EmptySubjectSpecification : Specification<Subject>
    {
    }
}
