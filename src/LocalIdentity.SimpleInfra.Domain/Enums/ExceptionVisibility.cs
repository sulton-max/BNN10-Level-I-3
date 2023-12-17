namespace LocalIdentity.SimpleInfra.Domain.Enums;

/// <summary>
///     Represents the visibility of an exception
/// </summary>
public enum ExceptionVisibility
{
    /// <summary>
    ///     Indicates that the exception can be seen by client and users
    /// </summary>
    Public,

    /// <summary>
    ///     Indicates that the exception must not leave the system and can be logged
    /// </summary>
    Protected,

    /// <summary>
    ///     Indicates that the exception must not leave the system and can be logged
    /// </summary>
    Private
}