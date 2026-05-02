namespace CoreFitness.Application.Authentication.Models;

public enum AuthenticationResultType
{
    Failed,
    SignedIn,
    RequiresVerification,
    InvalidCode,
    RequiresAccountCreation
}