using CoreFitness.Domain.Entities.Users.ValueObjects;
using CoreFitness.Domain.Enums;
using CoreFitness.Domain.Interfaces.UnitOfWork;
using CoreFitness.Domain.Interfaces.Users;
using CoreFitness.Infrastructure.Identity;
using CoreFitness.web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoreFitness.web.Controllers;

public class AuthController(
    SignInManager<ApplicationUser> signInManager, 
    UserManager<ApplicationUser> userManager, 
    ILogger<AuthController> logger, 
    IUserRepository userRepository, 
    IUnitOfWork unitOfWork) : Controller
{
    [HttpGet]
    public async Task<IActionResult> SignIn(string? returnUrl = null)
    {
        var schemes = await signInManager.GetExternalAuthenticationSchemesAsync();

        var vm = new SignInViewModel
        {
            ReturnUrl = returnUrl,
            ExternalProviders = [.. schemes.Select(x => x.Name)]
        };

        return View(vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult ExternalLogin(string provider, string? returnUrl = null)
    {
        if (string.IsNullOrWhiteSpace(provider))
            return RedirectToAction(nameof(SignIn), new { returnUrl });

        var redirectUrl = Url.Action(nameof(ExternalLogInCallback), "Auth", new { returnUrl });
        var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

        return Challenge(properties, provider);
    }

    [HttpGet]
    public async Task<IActionResult> ExternalLogInCallback(string? returnUrl = null, string? remoteError = null)
    {
        if (remoteError is not null)
        {
            logger.LogWarning("Remote error from provider: {Error}", remoteError);
            return ExternalLogInFailed(returnUrl);
        }

        var externalUser = await GetExternalUserInfo();

        if (externalUser is null)
            return ExternalLogInFailed(returnUrl);

        var (info, email) = externalUser.Value;

        var result = await signInManager.ExternalLoginSignInAsync(
            info.LoginProvider,
            info.ProviderKey,
            isPersistent: false,
            bypassTwoFactor: true);

        if (result.Succeeded)
            return RedirectToLocal(returnUrl);          

        return await ExternalVerification(email, returnUrl);
    }

    private async Task<IActionResult> ExternalVerification(string email, string? returnUrl = null)
    {
        return View("VerifyExternalLogIn", new VerifyExternalLogInViewModel
        {
            ReturnUrl = returnUrl,
            Email = email
        });
    }

#if DEBUG
    [HttpGet]
    public IActionResult TestVerifyExternalLogIn()
    {
        return View("VerifyExternalLogin", new VerifyExternalLogInViewModel
        {
            Email = "test@domain.com",
            ReturnUrl = "/"
        });
    }
#endif

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> VerifyExternalLogIn(VerifyExternalLogInViewModel vm)
    {
        if (!ModelState.IsValid)
            return View("VerifyExternalLogin", vm);

        if (!string.Equals(vm.Code, "123456", StringComparison.Ordinal))
        {
            ModelState.AddModelError(nameof(vm.Code), "Wrong code");

            return View("VerifyExternalLogin", vm);
        }

        var externalUser = await GetExternalUserInfo();

        if (externalUser is null)
            return ExternalLogInFailed(vm.ReturnUrl);

        var (info, email) = externalUser.Value;

        var existingUser = await userManager.FindByEmailAsync(email);

        if (existingUser is not null)
            return await LinkExistingUser(existingUser, info, vm.ReturnUrl);

        return await CreateExternalUser(email, info, vm.ReturnUrl);
    }

    private async Task<IActionResult> CreateExternalUser(string email, ExternalLoginInfo info, string? returnUrl = null)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var createResult = await userManager.CreateAsync(user);

        if (!createResult.Succeeded)
        {
            logger.LogError("Failed to create user {Email} : {Errors}",
                email,
                string.Join(",", createResult.Errors.Select(e => e.Description)));

            return ExternalLogInFailed(returnUrl);
        }

        var linkResult = await userManager.AddLoginAsync(user, info);

        if (!linkResult.Succeeded)
        {
            logger.LogError("Failed to link {Provider} to {Email} : {Errors}",
                info.LoginProvider,
                user.Email,
                string.Join(",", linkResult.Errors.Select(e => e.Description)));

            return ExternalLogInFailed(returnUrl);
        }

        var photoUrl = info.Principal.Claims.FirstOrDefault(c => c.Type == "picture")?.Value;
        var firstName = info.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value ?? "";
        var lastName = info.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value ?? "";

        var coreUser = CoreFitness.Domain.Entities.Users.User.Create(
            AuthenticationId.Create(user.Id.ToString()),
            UserEmail.Create(email),
            UserName.Create(firstName, lastName),
            null,
            photoUrl,
            UserRole.Member);

        await userRepository.AddAsync(coreUser);
        await unitOfWork.SaveChangesAsync();

        await signInManager.SignInAsync(user, isPersistent: false);
        return RedirectToLocal(returnUrl);
    }

    private async Task<IActionResult> LinkExistingUser(ApplicationUser user, ExternalLoginInfo info, string? returnUrl = null)
    {
        var result = await userManager.AddLoginAsync(user, info);

        if (!result.Succeeded)
        {
            logger.LogError("Failed to link {Provider} to {Email} : {Errors}",
                info.LoginProvider,
                user.Email,
                string.Join(",", result.Errors.Select(e => e.Description)));

            return ExternalLogInFailed(returnUrl);
        }

        await signInManager.SignInAsync(user, isPersistent: false);
        return RedirectToLocal(returnUrl);
    }

    private async Task<(ExternalLoginInfo Info, string Email)?> GetExternalUserInfo()
    {
        var info = await signInManager.GetExternalLoginInfoAsync();

        if (info is null)
        {
            logger.LogWarning("External login info was null!");

            return null;
        }

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrWhiteSpace(email))
        {
            logger.LogWarning("No email claim from {Provider}", info.LoginProvider);

            return null;
        }

        return (info, email);
    }

    private RedirectToActionResult ExternalLogInFailed(string? returnUrl = null)
    {
        TempData["Error"] = "Failed to login. Please try again";

        return RedirectToAction(nameof(SignIn), new { returnUrl });
    }

    private IActionResult RedirectToLocal(string? returnUrl = null)
    {
        if (Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("index", "Home");
    }
}