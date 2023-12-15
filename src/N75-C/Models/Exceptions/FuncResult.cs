namespace N75_C.Models.Exceptions;

public class FuncResult<T>
{
    public T Data { get; init; }

    public Exception? Exception { get; }

    public bool IsSuccess => Exception is null;

    public FuncResult(T data) => Data = data;

    public FuncResult(Exception exception) => Exception = exception;
}