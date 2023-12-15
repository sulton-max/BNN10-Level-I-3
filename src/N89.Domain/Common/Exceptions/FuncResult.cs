namespace AirBnb.Domain.Common.Exceptions;

/// <summary>
/// Represents result of function for result pattern
/// </summary>
/// <typeparam name="T"></typeparam>
public class FuncResult<T>
{
    /// <summary>
    /// Gets result of function
    /// </summary>
    public T Data { get; }

    /// <summary>
    /// Gets the exception associated with the property.
    /// </summary>
    public Exception? Exception { get; }

    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    /// <returns>`true` if operation is successful, otherwise `false`</returns>
    public bool IsSuccess => Exception is null;

    public FuncResult(T data) => Data = data;

    public FuncResult(Exception exception) => Exception = exception;
}