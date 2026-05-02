namespace CoreFitness.Application.Authentication.Models;

public enum PasswordSignInResult
{
    Succeeded,
    Failed,
    LockedOut,
    RequiresTwoFactor
}