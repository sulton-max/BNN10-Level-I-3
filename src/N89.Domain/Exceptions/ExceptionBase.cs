using AirBnb.Domain.Common.Exceptions;

namespace AirBnb.Domain.Exceptions;

/// <summary>
/// Represents a base exception class.
/// </summary>
public abstract class ExceptionBase(
    string? message = default,
    Exception? innerException = default,
    ExceptionVisibility visibility = ExceptionVisibility.Public
) : Exception(message, innerException)
{
    /// <summary>
    /// Gets or sets the visibility of the exception.
    /// </summary>
    public ExceptionVisibility Visibility { get; set; } = visibility;
}