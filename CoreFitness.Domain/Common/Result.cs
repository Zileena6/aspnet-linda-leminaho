namespace CoreFitness.Domain.Common;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }

    protected Result(bool isSuccess, Error? error)
    {
        if (isSuccess && error is not null)
            throw new InvalidOperationException("Success result cannot have error");

        if (!isSuccess && error is null)
            throw new InvalidOperationException("Failure result must have error");

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, null);

    public static Result Failure(Error error) => new(false, error);

    public static Result NotFound(string entity, object id) =>
        Failure(Error.NotFound(entity, id));

    public static Result Validation(string message) =>
        Failure(Error.Validation(message));

    public static Result Conflict(string message) =>
        Failure(Error.Conflict(message));
}