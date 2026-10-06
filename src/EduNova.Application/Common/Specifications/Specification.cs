using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Common.Specifications;

/// <summary>
/// Builder-style base for EF Core read specifications.
/// Criteria are ANDed into a single Where; orderings compose into
/// OrderBy/ThenBy; includes are applied before ordering.
/// Specifications must never call Skip/Take — pagination lives only in
/// QueryableExtensions.ToPagedResultAsync, which counts the filtered set
/// (TotalCount) and then applies Skip/Take for the requested page.
/// </summary>
public abstract class Specification<T> : ISpecification<T>
    where T : class
{
    private readonly List<Expression<Func<T, bool>>> _criteria = [];
    private readonly List<(Expression<Func<T, object>> KeySelector, bool Descending)> _orderings = [];
    private readonly List<Expression<Func<T, object>>> _includes = [];

    protected void Where(Expression<Func<T, bool>> criterion) => _criteria.Add(criterion);

    /// <summary>Adds an ordering key. First call becomes OrderBy, the rest ThenBy.</summary>
    protected void OrderBy(Expression<Func<T, object>> keySelector, bool descending = false) =>
        _orderings.Add((keySelector, descending));

    /// <summary>Alias of OrderBy for readability in multi-key specifications.</summary>
    protected void ThenBy(Expression<Func<T, object>> keySelector, bool descending = false) =>
        _orderings.Add((keySelector, descending));

    /// <summary>
    /// First-level navigation to load. Not needed when the handler projects
    /// with Select right after applying the specification — EF ignores
    /// includes once a projection exists.
    /// </summary>
    protected void Include(Expression<Func<T, object>> includePath) => _includes.Add(includePath);

    public IQueryable<T> ApplyTo(IQueryable<T> query)
    {
        foreach (var criterion in _criteria)
            query = query.Where(criterion);

        foreach (var include in _includes)
            query = query.Include(include);

        if (_orderings.Count > 0)
        {
            var (firstKey, firstDescending) = _orderings[0];
            var ordered = firstDescending
                ? query.OrderByDescending(firstKey)
                : query.OrderBy(firstKey);

            foreach (var (keySelector, descending) in _orderings.Skip(1))
                ordered = descending
                    ? ordered.ThenByDescending(keySelector)
                    : ordered.ThenBy(keySelector);

            query = ordered;
        }

        return query;
    }
}
