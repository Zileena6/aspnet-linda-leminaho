using CoreFitness.Application.DTOs.Auth;
using CoreFitness.Application.Identity;
using CoreFitness.Domain.Common;
using CoreFitness.Domain.Entities.Users;
using CoreFitness.Domain.Entities.Users.ValueObjects;
using CoreFitness.Domain.Enums;
using CoreFitness.Domain.Interfaces.UnitOfWork;
using CoreFitness.Domain.Interfaces.Users;
using Microsoft.AspNetCore.Identity;

namespace CoreFitness.Infrastructure.Identity;

public class AuthService(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork) : IAuthService
{
    public async Task<Result> LoginAsync(LoginDTO dto, CancellationToken ct = default)
    {
        var appUser = await userManager.FindByEmailAsync(dto.Email);
        if (appUser is null)
            return Result.Failure(Error.Validation("Invalid email or password"));

        var result = await signInManager.PasswordSignInAsync(appUser, dto.Password, dto.RememberMe, lockoutOnFailure: false);
        if (!result.Succeeded)
            return Result.Failure(Error.Validation("Invalid email or password"));

        return Result.Success();
    }

    public async Task LogOutAsync() =>
        await signInManager.SignOutAsync();

    public async Task<Result> RegisterAsync(RegisterDTO dto, CancellationToken ct = default)
    {
        if (await userManager.FindByEmailAsync(dto.Email) is not null)
            return Result.Failure(Error.Conflict("Email already in use"));

        var appUser = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email
        };

        var result = await userManager.CreateAsync(appUser, dto.Password);
        if (!result.Succeeded)
            return Result.Failure(Error.Validation(result.Errors.First().Description));

        await userManager.AddToRoleAsync(appUser, "Member");

        var domainUser = User.Create(
            new AuthenticationId(appUser.Id.ToString()),
            UserEmail.Create(dto.Email),
            UserName.Create(dto.FirstName, dto.LastName),
            null,
            null,
            UserRole.Member);

        await userRepository.AddAsync(domainUser, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}