using CoreFitness.Domain.Entities.Users.ValueObjects;
using System.Security.Claims;

namespace CoreFitness.web.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static AuthenticationId GetAuthenticationId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(value))
            throw new UnauthorizedAccessException("User Id claim is missing");

        return AuthenticationId.Create(value);
    }
}