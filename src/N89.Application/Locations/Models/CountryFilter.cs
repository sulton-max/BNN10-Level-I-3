using AirBnb.Domain.Common.Caching;
using AirBnb.Domain.Common.Query;
using AirBnb.Domain.Entities;
using AirBnb.Domain.Enums;

namespace AirBnb.Application.Locations.Models;

/// <summary>
/// Represents a country filter
/// </summary>
public class CountryFilter : FilterPagination, IQueryConvertible<Location>
{
    /// <summary>
    /// Gets the search keyword for country filtering
    /// </summary>
    public string? SearchKeyword { get; init; }

    public bool IncludeCities { get; set; }

    public QuerySpecification<Location> ToQuerySpecification()
    {
        var querySpecification = new QuerySpecification<Location>(PageSize, PageToken, true, GetHashCode());

        if (IncludeCities)
            querySpecification.IncludingOptions.Add(location => location.Cities);

        querySpecification.FilteringOptions.Add(location => location.Type == LocationType.Country);

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