namespace EduNova.Application.Common.Specifications;

/// <summary>
/// Read-query contract: describes WHAT data is needed (filters, search,
/// includes, ordering) — never HOW it is paged. Apply it to a DbSet/IQueryable,
/// then page the result with QueryableExtensions.ToPagedResultAsync.
/// </summary>
public interface ISpecification<T>
    where T : class
{
    /// <summary>
    /// Returns the input query with this specification's filtering, includes
    /// and ordering applied. The query stays server-side (composable) —
    /// nothing is executed or materialized here.
    /// </summary>
    IQueryable<T> ApplyTo(IQueryable<T> query);
}
