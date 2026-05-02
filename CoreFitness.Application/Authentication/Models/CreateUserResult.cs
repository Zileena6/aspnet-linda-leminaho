namespace CoreFitness.Application.Authentication.Models;

public record CreateUserResult
(
    bool Succeeded,
    string? UserId,
    string? Error
)
{
    public static CreateUserResult Success(string userId) =>
        new(true, userId, null);

    public static CreateUserResult Failed(string? error = null) =>
        new(false, null, error);
}