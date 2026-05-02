namespace CoreFitness.Application.Authentication.Models;

public sealed record LinkResult(bool Succeeded, bool IsAlreadyLinked = false, string? Error = null)
{
    public static LinkResult Success() =>
        new(true);

    public static LinkResult Failed(string? error = null) =>
        new(false, Error: error);

    public static LinkResult AlreadyLinked() =>
        new(true, IsAlreadyLinked: true);
}