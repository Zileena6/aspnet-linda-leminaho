using CoreFitness.Application.Authentication;
using CoreFitness.Application.Authentication.Models;
using CoreFitness.Infrastructure.Identity;
using CoreFitness.Web.ViewModels.Auth;
using CoreFitness.Web.ViewModels.Profile;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Web.Controllers;

public class AuthController(IAuthService authService, SignInManager<ApplicationUser> signInManager) : Controller
{
    // [HttpPost]
    // public IActionResult ExternalLogin(string provider, string returnUrl)
    // {
    //     var redirectUrl = Url.Action("ExternalLogInCallback", "Auth", new { returnUrl });

    //     var properties = new AuthenticationProperties
    //     {
    //         RedirectUri = redirectUrl
    //     };

    //     return Challenge(properties, provider);
    // }

    [HttpPost]
        public IActionResult ExternalLogin(string provider, string? returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLogInCallback", "Auth", null, Request.Scheme);

            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

           return Challenge(properties, provider);
        }

    [HttpGet]
    public async Task<IActionResult> ExternalLogInCallback(string? returnUrl = null, string? remoteError = null, CancellationToken ct = default)
    {
        var result = await authService.HandleExternalCallbackAsync(returnUrl, remoteError, confirmed: false, ct);

        return result.Type switch
        {
            AuthenticationResultType.SignedIn => RedirectToAction("Index", "Profile"),
            AuthenticationResultType.RequiresVerification when result.Email is not null => View("VerifyEmail", new VerifyEmailViewModel
            {
                Email = result.Email,
                ReturnUrl = returnUrl
            }),
            AuthenticationResultType.RequiresAccountCreation when result.Email is not null => View("ConfirmExternalAccount", new ConfirmExternalAccountViewModel
            {
                Email = result.Email,
                ReturnUrl = returnUrl
            }),
            _ => ExternalLogInFailed(returnUrl)
        };
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

    [HttpPost]
    public async Task<IActionResult> VerifyEmailLogIn(VerifyEmailViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var result = await authService.VerifyEmailAsync(vm.Email, vm.Code, vm.ReturnUrl);

        if (result.Type == AuthenticationResultType.InvalidCode)
        {
            ModelState.AddModelError(nameof(vm.Code), "Wrong code");

            return View(vm);
        }

        return result.Type switch
        {
            AuthenticationResultType.SignedIn => RedirectToLocal(result.ReturnUrl),
            _ => ExternalLogInFailed(result.ReturnUrl)
        };
    }

    [HttpPost]
    public async Task<IActionResult> StartExternalVerification(NoAccountFoundViewModel vm)
    {
        return View("VerifyExternalLogin", new VerifyExternalLogInViewModel
        {
            Email = vm.Email,
            ReturnUrl = vm.ReturnUrl
        });
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmExternalAccount(ConfirmExternalAccountViewModel vm, CancellationToken ct = default)
    {
        var result = await authService.HandleExternalCallbackAsync(vm.ReturnUrl, null, confirmed: true, ct);

        return result.Type switch
        {
            AuthenticationResultType.SignedIn => RedirectToAction("Index", "Profile"),
            _ => ExternalLogInFailed(vm.ReturnUrl)
        };
    }

    private RedirectToActionResult ExternalLogInFailed(string? returnUrl = null)
    {
        TempData["Error"] = "Failed to login. Please try again";

        return RedirectToAction("SignIn", "Account", new { returnUrl });
    }

    private IActionResult RedirectToLocal(string? returnUrl = null)
    {
        if (Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public new async Task<IActionResult> SignOut()
    {
        await authService.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }
}