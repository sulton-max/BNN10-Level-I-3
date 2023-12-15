using AirBnb.Domain.Common.Query;
using AirBnb.Domain.Entities;

namespace AirBnb.Persistence.Repositories.Interfaces;

/// <summary>
/// Defines location repository functionalities
/// </summary>
public interface ILocationRepository
{
    /// <summary>
    /// Retrieves a list of locations based on the specified query specification asynchronously.
    /// </summary>
    /// <param name="querySpecification">The query specification to apply.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>Collection of matching locations.</returns>
    ValueTask<IList<Location>> GetAsync(QuerySpecification<Location> querySpecification, CancellationToken cancellationToken = default);
}