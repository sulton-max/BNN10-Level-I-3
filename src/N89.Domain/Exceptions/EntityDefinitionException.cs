using AirBnb.Domain.Common.Exceptions;

namespace AirBnb.Domain.Exceptions;

/// <summary>
/// Represents entity definition exception
/// </summary>
public class EntityDefinitionException<TEntity>(
    Guid entityId,
    Exception? innerException = default,
    ExceptionVisibility visibility = ExceptionVisibility.Protected
) : EntityExceptionBase(
    entityId,
    $"Entity {typeof(TEntity).Name} definition with id {entityId} doesn't match with entries",
    innerException,
    visibility
);