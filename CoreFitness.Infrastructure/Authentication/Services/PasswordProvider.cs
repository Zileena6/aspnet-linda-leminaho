using CoreFitness.Application.Authentication.Abstractions;
using CoreFitness.Application.Authentication.Models;
using CoreFitness.Domain.Common;
using CoreFitness.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CoreFitness.Infrastructure.Authentication.Services;

public class PasswordProvider(
    UserManager<ApplicationUser> userManager, 
    SignInManager<ApplicationUser> signInManager, 
    ILogger<PasswordProvider> logger) : IPasswordProvider
{
    public async Task<PasswordSignInResult> PasswordSignInAsync(
        string email, 
        string password, 
        bool rememberMe, 
        CancellationToken ct = default)
    {
        var result = await signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: true);

        return result switch
        {
            { Succeeded: true } => PasswordSignInResult.Succeeded,
            { RequiresTwoFactor: true } => PasswordSignInResult.RequiresTwoFactor,
            { IsLockedOut: true } => PasswordSignInResult.LockedOut,
            _ => PasswordSignInResult.Failed
        };
    }

    public async Task<CreateUserResult> CreateUserWithPasswordAsync(
        string email, 
        string? password = null, 
        CancellationToken ct = default)
    {
        var existing = await userManager.FindByEmailAsync(email);

        if (existing is not null)
        {
            logger.LogWarning("User already exists for {Email}", email);

            return CreateUserResult.Failed();
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = password is null
        };

        IdentityResult result = password is null
            ? await userManager.CreateAsync(user)
            : await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            logger.LogWarning("Failed to create user {Email}: {Errors}", email, string.Join(", ", result.Errors.Select(e => e.Description)));
            return CreateUserResult.Failed();
        }

        return CreateUserResult.Success(user.Id.ToString());
    }

    public async Task<PasswordSignInResult> SignInWithEmailAsync(string email, CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            logger.LogWarning("SignInByEmail failed. User not found: {Email}", email);
            return PasswordSignInResult.Failed;
        }

        await signInManager.SignInAsync(user, isPersistent: false);

        return PasswordSignInResult.Succeeded;
    }

    public async Task SignInAsync(string userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
        {
            logger.LogError("Signin failed. User not found: {UserId}", userId);
            return;
        }

        await signInManager.SignInAsync(user, isPersistent: false);
    }

    public async Task SignOutAsync(CancellationToken ct = default) =>
        await signInManager.SignOutAsync();

    public async Task<Result> DeleteUserAsync(string userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
        {
            logger.LogWarning("Failed to delete user. User not found: {UserId}", userId);
            return Result.Failure(Error.NotFound("User", userId));
        }

        var result = await userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));

            logger.LogError("Failed to delete user {UserId}: {Errors}", userId, errors);

            return Result.Failure(Error.Failure(errors));
        }

        return Result.Success();
    }
}