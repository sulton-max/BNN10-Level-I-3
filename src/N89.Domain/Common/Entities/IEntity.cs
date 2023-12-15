namespace AirBnb.Domain.Common.Entities;

/// <summary>
/// Defines common entity properties
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Gets or sets entity id
    /// </summary>
    Guid Id { get; set; }
}