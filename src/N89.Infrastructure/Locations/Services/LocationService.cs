using System.Linq.Expressions;
using AirBnb.Application.Locations.Services;
using AirBnb.Domain.Common.Query;
using AirBnb.Domain.Entities;
using AirBnb.Persistence.Repositories.Interfaces;

namespace AirBnb.Infrastructure.Locations.Services;

/// <summary>
/// Represents location foundation service functionality
/// </summary>
public class LocationService(ILocationRepository locationRepository) : ILocationService
{
    public ValueTask<IList<Location>> GetAsync(QuerySpecification<Location> querySpecification, CancellationToken cancellationToken = default)
    {
        return locationRepository.GetAsync(querySpecification, cancellationToken);
    }
}