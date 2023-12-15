using AirBnb.Domain.Common.Entities;
using AirBnb.Domain.Enums;

namespace AirBnb.Domain.Entities;

/// <summary>
/// Represents unit of location 
/// </summary>
public class Location : Entity
{
    /// <summary>
    /// Gets or sets location name
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Gets or sets location code
    /// </summary>
    public string Code { get; set; } = default!;

    /// <summary>
    /// Gets or sets location type
    /// </summary>
    public LocationType Type { get; set; }

    /// <summary>
    /// Gets or sets the ID of the parent.
    /// </summary>
    public Guid? ParentId { get; set; }
    
    public IList<Location> Cities { get; set; }
}