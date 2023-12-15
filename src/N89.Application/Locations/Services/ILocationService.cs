using AirBnb.Domain.Common.Query;
using AirBnb.Domain.Entities;

namespace AirBnb.Application.Locations.Services;

/// <summary>
/// Defines location foundation service functionalities.
/// </summary>
public interface ILocationService
{
    /// <summary>
    /// Retrieves a list of locations based on the provided query specification.
    /// </summary>
    /// <param name="querySpecification">The query specification to apply.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>Collection of matching locations.</returns>
    ValueTask<IList<Location>> GetAsync(QuerySpecification<Location> querySpecification, CancellationToken cancellationToken = default);
}