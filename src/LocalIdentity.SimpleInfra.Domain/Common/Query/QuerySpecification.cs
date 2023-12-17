using System.Linq.Expressions;
using LocalIdentity.SimpleInfra.Domain.Common.Caching;
using LocalIdentity.SimpleInfra.Domain.Comparers;
using Microsoft.EntityFrameworkCore.Query;

namespace LocalIdentity.SimpleInfra.Domain.Common.Query;

/// <summary>
///     Represents a query specification for retrieving entities from a cache.
/// </summary>
/// <typeparam name="TSource">The type of source.</typeparam>
public class QuerySpecification<TSource>(uint pageSize, uint pageToken, bool asNoTracking, int? filterHashCode = default) : ICacheModel
{
    /// <summary>
    ///     Gets filtering options collection for query.
    /// </summary>
    public List<Expression<Func<TSource, bool>>> FilteringOptions { get; } = [];

    /// <summary>
    ///     Gets ordering options collection for query.
    /// </summary>
    public List<(Expression<Func<TSource, object>> KeySelector, bool IsAscending)> OrderingOptions { get; } = [];

    /// <summary>
    ///     Gets including options collection for query.
    /// </summary>
    public List<Expression<Func<TSource, object>>> IncludingOptions { get; } = [];

    /// <summary>
    ///     Gets pagination options for query.
    /// </summary>
    public FilterPagination PaginationOptions { get; } = new()
    {
        PageSize = pageSize,
        PageToken = pageToken
    };

    /// <summary>
    ///     Gets value indicating whether to use tracking in query
    /// </summary>
    public bool AsNoTracking { get; } = asNoTracking;

    /// <summary>
    ///     Gets the filter hash code that is used to filter items.
    /// </summary>
    public int? FilterHashCode { get; } = filterHashCode;

    public string CacheKey => $"{typeof(TSource).Name}_{GetHashCode()}";

    public override int GetHashCode()
    {
        if (FilterHashCode is not null) return FilterHashCode.Value;

        var hashCode = new HashCode();
        var expressionEqualityComparer = ExpressionEqualityComparer.Instance;

        foreach (var filteringExpression in FilteringOptions.Order(new PredicateExpressionComparer<TSource>()))
            hashCode.Add(expressionEqualityComparer.GetHashCode(filteringExpression));

        foreach (var includeExpression in IncludingOptions.Order(new KeySelectorExpressionComparer<TSource>()))
            hashCode.Add(expressionEqualityComparer.GetHashCode(includeExpression));

        foreach (var orderingExpression in OrderingOptions)
            hashCode.Add(expressionEqualityComparer.GetHashCode(orderingExpression.KeySelector));

        hashCode.Add(PaginationOptions);

        return hashCode.ToHashCode();
    }

    public override bool Equals(object? obj)
    {
        return obj is QuerySpecification<TSource> querySpecification && querySpecification.GetHashCode() == GetHashCode();
    }
}