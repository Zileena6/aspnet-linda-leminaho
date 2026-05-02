using CoreFitness.Application.Authentication.Abstractions;
using CoreFitness.Application.Authentication.Models;
using CoreFitness.Application.DTOs.Auth;
using CoreFitness.Domain.Entities.Users;
using CoreFitness.Domain.Entities.Users.ValueObjects;
using CoreFitness.Domain.Enums;
using CoreFitness.Domain.Interfaces.UnitOfWork;
using CoreFitness.Domain.Interfaces.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace CoreFitness.Application.Authentication.Services;

public class AuthService(IUserRepository userRepository, IPasswordProvider passwordProvider, IExternalAuthProvider externalAuthProvider, IUnitOfWork unitOfWork, ILogger<AuthService> logger) : IAuthService
{
    public AuthenticationProperties ConfigureExternalLogin(string provider, string redirectUrl) =>
        externalAuthProvider.ConfigureExternalLogin(provider, redirectUrl);

    public async Task<IReadOnlyList<string>> GetExternalProvidersAsync(CancellationToken ct = default) => await externalAuthProvider.GetExternalProvidersAsync(ct);

    public async Task<AuthenticationResult> LoginAsync(LoginDTO dto, CancellationToken ct = default)
    {
        var result = await passwordProvider.PasswordSignInAsync(dto.Email, dto.Password, false, ct);

        return result switch
        {
            PasswordSignInResult.Succeeded => AuthenticationResult.SignedIn(null),
            PasswordSignInResult.RequiresTwoFactor => AuthenticationResult.RequiresVerification(dto.Email, null),
            PasswordSignInResult.LockedOut => AuthenticationResult.Failed("Locked out"),
            _ => AuthenticationResult.Failed("Invalid login")
        };
    }

    public async Task<AuthenticationResult> HandleExternalCallbackAsync(string? returnUrl, string? remoteError, bool confirmed = false, CancellationToken ct = default)
    {
        if (!string.IsNullOrEmpty(remoteError))
            return AuthenticationResult.Failed(remoteError);

        var externalUser = await externalAuthProvider.GetExternalUserAsync(ct);

        if (externalUser is null)
        {
            logger.LogWarning("External user info nof found");
            return AuthenticationResult.Failed("Failed to get external user");
        }

        var signInResult = await externalAuthProvider.SignInWithExternalAsync(
            externalUser.Provider,
            externalUser.ProviderKey,
            ct
        );

        if (signInResult.Succeeded)
            return AuthenticationResult.SignedIn(returnUrl);

        if (string.IsNullOrWhiteSpace(externalUser.Email))
            return AuthenticationResult.Failed("Email not provided by external provider");

        var email = UserEmail.Create(externalUser.Email);

        var existingUser = await userRepository.GetByEmailAsync(email, ct);

        if (existingUser is not null)
        {
            var existingUserId = existingUser.AuthenticationId.Value;

            var linkResult = await externalAuthProvider.LinkExternalLoginAsync(
                externalUser.Provider,
                externalUser.ProviderKey,
                existingUserId,
                ct
            );

            if (!linkResult.Succeeded && !linkResult.IsAlreadyLinked)
                logger.LogWarning("Failed to link externa login for existing user {UserId}", existingUserId);

            await passwordProvider.SignInAsync(existingUserId, ct);

            return AuthenticationResult.SignedIn(returnUrl);
        }

        if (!confirmed)
            return AuthenticationResult.RequiresAccountCreation(externalUser.Email, returnUrl);

        var createResult = await passwordProvider.CreateUserWithPasswordAsync(externalUser.Email, null, ct);

        if (!createResult.Succeeded || createResult.UserId is null)
            return AuthenticationResult.Failed(returnUrl);

        var userId = createResult.UserId;

        var domainUser = User.Create(
            AuthenticationId.Create(createResult.UserId),
            email,
            UserName.Create(externalUser.FirstName ?? "", externalUser.LastName ?? ""),
            null,
            externalUser.PhotoUrl,
            UserRole.Member
        );

        try
        {
            await userRepository.AddAsync(domainUser, ct);
            await unitOfWork.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to create domain user, rolling back: {Error}", ex.Message);
            await passwordProvider.DeleteUserAsync(userId, ct);
            return AuthenticationResult.Failed("Failed to create account");
        }


        var linkResultNewUser = await externalAuthProvider.LinkExternalLoginAsync(
            externalUser.Provider,
            externalUser.ProviderKey,
            userId,
            ct
        );

        if (!linkResultNewUser.Succeeded && !linkResultNewUser.IsAlreadyLinked)
        {
            logger.LogError("Failed to link external login for {Provider} and user {UserId}", externalUser.Provider, userId);

            await passwordProvider.DeleteUserAsync(userId, ct);
            await userRepository.DeleteAsync(domainUser.Id, ct);
            await unitOfWork.SaveChangesAsync(ct);
            return AuthenticationResult.Failed("Failed to link external login");
        }

        await passwordProvider.SignInAsync(userId, ct);

        return AuthenticationResult.SignedIn(returnUrl);
    }

    public async Task<AuthenticationResult> VerifyEmailAsync(string email, string code, string? returnUrl, CancellationToken ct = default)
    {
        if (!string.Equals(code, "123456", StringComparison.Ordinal))
            return AuthenticationResult.InvalidCode(null);

        var result = await passwordProvider.SignInWithEmailAsync(email, ct);

        return result == PasswordSignInResult.Succeeded
            ? AuthenticationResult.SignedIn(null)
            : AuthenticationResult.Failed(null);
    }

    public async Task<AuthenticationResult> RegisterAsync(RegisterDTO dto, CancellationToken ct = default)
    {
        var result = await passwordProvider.CreateUserWithPasswordAsync(dto.Email, dto.Password, ct);

        if (!result.Succeeded || result.UserId is null)
            return AuthenticationResult.Failed(result.Error);

        var domainUser = User.Create(
           AuthenticationId.Create(result.UserId),
           UserEmail.Create(dto.Email),
           UserName.Create(dto.FirstName, dto.LastName),
           null,
           null,
           UserRole.Member
        );

        try
        {
            await userRepository.AddAsync(domainUser, ct);
            await unitOfWork.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to create domain user, rolling back: {Error}", ex.Message);
            await passwordProvider.DeleteUserAsync(result.UserId, ct);
            return AuthenticationResult.Failed("Failed to create account");
        }

        await passwordProvider.SignInAsync(result.UserId, ct);

        return AuthenticationResult.SignedIn(null);
    }

    public async Task SignOutAsync(CancellationToken ct = default) =>
        await passwordProvider.SignOutAsync(ct);

}