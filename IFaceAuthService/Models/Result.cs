using IFaceAuthService.DTOs.Out.Errors;

namespace IFaceAuthService.Models;

public class Result<T> : Result
{
    public T? Value { get; set; }

    public bool HasValue()
    {

        return Value != null;
    }
}

public class Result
{
    public ErrorModel? Error { get; set; }

    public Result<T> Cast<T>(T t)
    {
        return new Result<T>
        {
            Value = t,
            Error = Error,
        };
    }


    public bool HasError()
    {
        return Error != null;
    }

    protected Result() { }

    public static Result<T> Success<T>(T value)
    {
        return new Result<T> { Value = value };
    }




    public static Result Empty()
    {
        return new Result();
    }

    public static Result<T> ForError<T>(ErrorModel error)
    {
        return new Result<T> { Error = error };
    }

    public static Result ForError(ErrorModel error)
    {
        return new Result { Error = error };
    }

    public static Result<T> ForError<T>(Exception exception)
    {
        return new Result<T>
        {
            Error = new ErrorModel(exception.InnerException?.Message ?? exception.Message)
        };
    }

    public static Result ForError(Exception exception)
    {
        return new Result { Error = new ErrorModel(exception.Message) };
    }

}