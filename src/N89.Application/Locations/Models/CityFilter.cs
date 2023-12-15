using AirBnb.Domain.Common.Query;
using AirBnb.Domain.Entities;
using AirBnb.Domain.Enums;

namespace AirBnb.Application.Locations.Models;

/// <summary>
/// Represents a city filter
/// </summary>
public class CityFilter : FilterPagination, IQueryConvertible<Location>
{
    /// <summary>
    /// Gets the search keyword for city filtering
    /// </summary>
    public string? SearchKeyword { get; init; }

    public QuerySpecification<Location> ToQuerySpecification()
    {
        var querySpecification = new QuerySpecification<Location>(PageSize, PageToken, true, GetHashCode());

        querySpecification.FilteringOptions.Add(location => location.Type == LocationType.City);

        if (SearchKeyword is not null)
            querySpecification.FilteringOptions.Add(location => location.Name.ToLower().Contains(SearchKeyword.ToLower()));

        return querySpecification;
    }

    public override bool Equals(object? obj)
    {
        return obj is CountryFilter countryFilter && countryFilter.GetHashCode() == GetHashCode();
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();

        hashCode.Add(SearchKeyword);
        hashCode.Add(PageSize);
        hashCode.Add(PageToken);

        return hashCode.ToHashCode();
    }
}