using AirBnb.Domain.Common.Entities;
using AirBnb.Domain.Common.Query;
using Microsoft.EntityFrameworkCore;

namespace AirBnb.Persistence.Extensions;

/// <summary>
/// Extension methods for LINQ operations.
/// </summary>
public static class LinqExtensions
{
    /// <summary>
    /// Applies the query specification to queryable source
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the queryable source.</typeparam>
    /// <param name="source">Queryable source to apply specifications to.</param>
    /// <param name="querySpecification">The query specification to apply.</param>
    /// <returns>Same queryable resource with specifications applied</returns>
    public static IQueryable<TSource> ApplySpecification<TSource>(this IQueryable<TSource> source, QuerySpecification<TSource> querySpecification)
        where TSource : class, IEntity
    {
        source = source
            .ApplyPredicates(querySpecification)
            .ApplyOrdering(querySpecification)
            .ApplyIncluding(querySpecification)
            .ApplyPagination(querySpecification);

        return source;
    }

    /// <summary>
    /// Applies the query specification to enumerable source
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the enumerable source.</typeparam>
    /// <param name="source">Enumerable source to apply specifications to.</param>
    /// <param name="querySpecification">The query specification to apply.</param>
    /// <returns>Same enumerable resource with specifications applied</returns>
    public static IEnumerable<TSource> ApplySpecification<TSource>(this IEnumerable<TSource> source, QuerySpecification<TSource> querySpecification)
        where TSource : IEntity
    {
        source = source.ApplyPredicates(querySpecification).ApplyOrdering(querySpecification).ApplyPagination(querySpecification);

        return source;
    }

    /// <summary>
    /// Applies the predicates to queryable source
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the queryable source.</typeparam>
    /// <param name="source">Queryable source to apply specifications to.</param>
    /// <param name="querySpecification">The query specification to apply.</param>
    /// <returns>Same queryable resource with predicates applied</returns>
    public static IQueryable<TSource> ApplyPredicates<TSource>(this IQueryable<TSource> source, QuerySpecification<TSource> querySpecification)
        where TSource : IEntity
    {
        querySpecification.FilteringOptions.ForEach(predicate => source = source.Where(predicate));

        return source;
    }

    /// <summary>
    /// Applies the predicates to enumerable source
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the enumerable source.</typeparam>
    /// <param name="source">Enumerable source to apply specifications to.</param>
    /// <param name="querySpecification">The query specification to apply.</param>
    /// <returns>Same enumerable resource with predicates applied</returns>
    public static IEnumerable<TSource> ApplyPredicates<TSource>(this IEnumerable<TSource> source, QuerySpecification<TSource> querySpecification)
        where TSource : IEntity
    {
        querySpecification.FilteringOptions.ForEach(predicate => source = source.Where(predicate.Compile()));

        return source;
    }

    /// <summary>
    /// Applies the joining to queryable source
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the queryable source.</typeparam>
    /// <param name="source">Queryable source to apply specifications to.</param>
    /// <param name="querySpecification">The query specification to apply.</param>
    /// <returns>Same queryable resource with predicates applied</returns>
    public static IQueryable<TSource> ApplyIncluding<TSource>(this IQueryable<TSource> source, QuerySpecification<TSource> querySpecification)
        where TSource : class, IEntity
    {
        querySpecification.IncludingOptions.ForEach(includeOption => source = source.Include(includeOption));
    
        return source;
    }

    /// <summary>
    /// Applies ordering to queryable source
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the queryable source.</typeparam>
    /// <param name="source">Queryable source to apply specifications to.</param>
    /// <param name="querySpecification">The query specification to apply.</param>
    /// <returns>Same queryable resource with ordering applied</returns>
    public static IQueryable<TSource> ApplyOrdering<TSource>(this IQueryable<TSource> source, QuerySpecification<TSource> querySpecification)
        where TSource : IEntity
    {
        if (querySpecification.OrderingOptions.Count == 0)
            return source.OrderBy(entity => entity.Id);

        querySpecification.OrderingOptions.ForEach(
            orderByExpression => source = orderByExpression.IsAscending
                ? source.OrderBy(orderByExpression.Item1)
                : source.OrderByDescending(orderByExpression.Item1)
        );

        return source;
    }

    /// <summary>
    /// Applies ordering to enumerable source
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the enumerable source.</typeparam>
    /// <param name="source">Enumerable source to apply specifications to.</param>
    /// <param name="querySpecification">The query specification to apply.</param>
    /// <returns>Same enumerable resource with ordering applied</returns>
    public static IEnumerable<TSource> ApplyOrdering<TSource>(this IEnumerable<TSource> source, QuerySpecification<TSource> querySpecification)
        where TSource : IEntity
    {
        if (querySpecification.OrderingOptions.Count == 0)
            return source.OrderBy(entity => entity.Id);

        querySpecification.OrderingOptions.ForEach(
            orderByExpression => source = orderByExpression.IsAscending
                ? source.OrderBy(orderByExpression.Item1.Compile())
                : source.OrderByDescending(orderByExpression.Item1.Compile())
        );

        return source;
    }

    /// <summary>
    /// Applies pagination to queryable source
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the queryable source.</typeparam>
    /// <param name="source">Queryable source to apply specifications to.</param>
    /// <param name="querySpecification">The query specification to apply.</param>
    /// <returns>Same queryable resource with pagination applied</returns>
    public static IQueryable<TSource> ApplyPagination<TSource>(this IQueryable<TSource> source, QuerySpecification<TSource> querySpecification)
        where TSource : IEntity
    {
        return source.Skip((int)((querySpecification.PaginationOptions.PageToken - 1) * querySpecification.PaginationOptions.PageSize))
            .Take((int)querySpecification.PaginationOptions.PageSize);
    }

    /// <summary>
    /// Applies pagination to queryable source
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the enumerable source.</typeparam>
    /// <param name="source">Enumerable source to apply specifications to.</param>
    /// <param name="querySpecification">The query specification to apply.</param>
    /// <returns>Same enumerable resource with pagination applied</returns>
    public static IEnumerable<TSource> ApplyPagination<TSource>(this IEnumerable<TSource> source, QuerySpecification<TSource> querySpecification)
        where TSource : IEntity
    {
        return source.Skip((int)((querySpecification.PaginationOptions.PageToken - 1) * querySpecification.PaginationOptions.PageSize))
            .Take((int)querySpecification.PaginationOptions.PageSize);
    }
}