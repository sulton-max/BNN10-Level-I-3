namespace AirBnb.Domain.Common.Entities;

/// <summary>
/// Provides common entity properties
/// </summary>
public abstract class Entity : IEntity
{
    public Guid Id { get; set; }
}