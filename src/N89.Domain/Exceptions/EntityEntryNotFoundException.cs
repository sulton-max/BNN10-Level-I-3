using AirBnb.Domain.Common.Entities;
using AirBnb.Domain.Common.Exceptions;

namespace AirBnb.Domain.Exceptions;

/// <summary>
/// Represents entity not found exception
/// </summary>
public class EntityEntryNotFoundException<TEntity>(Guid entityId, ExceptionVisibility visibility = ExceptionVisibility.Protected)
    : EntityExceptionBase(entityId, $"Entity {typeof(TEntity).Name} with id {entityId} could not be found", null, visibility) where TEntity : IEntity;