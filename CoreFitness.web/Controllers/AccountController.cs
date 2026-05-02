using CoreFitness.Application.Authentication;
using CoreFitness.Application.Authentication.Models;
using CoreFitness.Application.DTOs.Auth;
using CoreFitness.web.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.web.Controllers;

public class AccountController(IAuthService authService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> SignUp(CancellationToken ct = default)
    {
        return View(new SignUpViewModel
        {
            ExternalProviders = await authService.GetExternalProvidersAsync(ct)
        });
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        TempData["VerifyEmail"] = vm.Email;

        return RedirectToAction(nameof(VerifyEmail), new { email = vm.Email });
    }

    [HttpGet]
    public IActionResult VerifyEmail(string email) => View(new VerifyEmailViewModel { Email = email });

    [HttpPost]
    public IActionResult VerifyEmail(VerifyEmailViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        if (!string.Equals(vm.Code, "123456", StringComparison.Ordinal))
        {
            ModelState.AddModelError(nameof(vm.Code), "Wrong code");
            return View(vm);
        }

        return RedirectToAction(nameof(SetPassword), new { email = vm.Email });
    }

    [HttpGet]
    public IActionResult SetPassword(string email) => View(new SetUpPasswordViewModel { Email = email });

    [HttpPost]
    public async Task<IActionResult> SetPassword(SetUpPasswordViewModel vm, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var result = await authService.RegisterAsync(new RegisterDTO
        {
            Email = vm.Email,
            Password = vm.Password,
            FirstName = "",
            LastName = ""
        }, ct);

        if (result.Type != AuthenticationResultType.SignedIn)
        {
            ModelState.AddModelError(string.Empty, "Failed to create account");
            return View(vm);
        }

        return RedirectToAction("Index", "Profile");
    }

    [HttpGet]
    public async Task<IActionResult> SignIn(string? returnUrl = null)
    {
        return View(new SignInViewModel
        {
            ReturnUrl = returnUrl,
            ExternalProviders = await authService.GetExternalProvidersAsync()
        });
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(SignInViewModel vm, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var result = await authService.LoginAsync(new LoginDTO
        {
            Email = vm.Email,
            Password = vm.Password
        }, ct);

        if (result.Type != AuthenticationResultType.SignedIn)
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password");
            return View(vm);
        }

        return RedirectToLocal(vm.ReturnUrl);
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }
}