using AirBnb.Domain.Common.Exceptions;

namespace AirBnb.Domain.Extensions;

public static class ExceptionExtensions
{
    public static async ValueTask<FuncResult<T>> GetValueAsync<T>(this Func<Task<T>> func)
    {
        FuncResult<T> result;

        try
        {
            result = new FuncResult<T>(await func());
        }
        catch (Exception e)
        {
            result = new FuncResult<T>(e);
        }

        return result;
    }

    public static async ValueTask<FuncResult<T>> GetValueAsync<T>(this Func<ValueTask<T>> func)
    {
        FuncResult<T> result;

        try
        {
            result = new FuncResult<T>(await func());
        }
        catch (Exception e)
        {
            result = new FuncResult<T>(e);
        }

        return result;
    }
}