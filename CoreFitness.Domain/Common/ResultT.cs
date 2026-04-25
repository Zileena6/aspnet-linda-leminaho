namespace CoreFitness.Domain.Common;

public sealed class Result<T> : Result
{
    private readonly T? _value;

    public T Value =>
        IsSuccess ? _value! : throw new InvalidOperationException("Cannot access value of a failed result");

    private Result(bool isSuccess, T? value, Error? error) : base(isSuccess, error)
    {
        _value = value;
    }

    public static Result<T> Success(T value) =>
        new(true, value, null);

    public static new Result<T> Failure(Error error) =>
        new(false, default, error);

    public static new Result<T> NotFound(string entity, object id) =>
        Failure(Error.NotFound(entity, id));

    public static new Result<T> Validation(string message) =>
        Failure(Error.Validation(message));

    public static new Result<T> Conflict(string message) =>
        Failure(Error.Conflict(message));
}