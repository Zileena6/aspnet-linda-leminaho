using CoreFitness.Application.Authentication;
using CoreFitness.Application.DTOs.User;
using CoreFitness.Application.Interfaces;
using CoreFitness.web.Extensions;
using CoreFitness.web.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.web.Controllers;

[Authorize]
public class ProfileController(IUserService userService, IAuthService authService, ILogger<ProfileController> logger) : Controller
{
    public async Task<IActionResult> Index()
    {
        var authId = User.GetAuthenticationId();

        var userResult = await userService.GetByAuthenticationId(authId);
        if (!userResult.IsSuccess)
        {
            await authService.SignOutAsync();

            TempData["Error"] = "Your account was not found. Please sign in again or create a new account";

            return RedirectToAction("SignIn", "Account");
        }

        var statsResult = await userService.GetStatisticsAsync(userResult.Value!.Id);

        var vm = new ProfilePageViewModel
        {
            Id = userResult.Value.Id,
            FirstName = userResult.Value.FirstName,
            LastName = userResult.Value.LastName,
            Email = userResult.Value.Email,
            PhoneNumber = userResult.Value.PhoneNumber,
            PhotoUrl = userResult.Value.PhotoUrl,
            Statistics = statsResult.IsSuccess ? statsResult.Value : null,
            Weight = statsResult.IsSuccess ? statsResult.Value?.CurrentWeight : null,
            Height = statsResult.IsSuccess ? statsResult.Value?.Height : null,
            TargetWeight = statsResult.IsSuccess ? statsResult.Value?.TargetWeight : null
        };

        return View(vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(ProfilePageViewModel vm, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return View("Index", vm);

        var dto = new UpdateProfileDTO
        {
            Id = vm.Id,
            FirstName = vm.FirstName,
            LastName = vm.LastName,
            Email = vm.Email,
            PhoneNumber = vm.PhoneNumber,
            Weight = vm.Weight,
            Height = vm.Height,
            TargetWeight = vm.TargetWeight,
        };

        var result = await userService.UpdateProfileAsync(dto, ct);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result?.Error?.Message ?? "Update failed");

            return View("Index", vm);
        }

        TempData["Success"] = "Profile updated successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(CancellationToken ct = default)
    {
        var authId = User.GetAuthenticationId();

        var result = await userService.DeleteAccountAsync(authId, ct);

        if (!result.IsSuccess)
        {
            TempData["Error"] = "Your account was not found.";

            return RedirectToAction(nameof(Index));
        }

        await authService.SignOutAsync(ct);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadPhoto(IFormFile photo, CancellationToken ct = default)
    {
        if (photo is null || photo.Length == 0)
            return RedirectToAction("Index");

        var authId = User.GetAuthenticationId();

        using var stream = photo.OpenReadStream();

        var result = await userService.UploadProfilePhotoAsync(
            authId,
            stream,
            photo.FileName,
            ct
        );

        if (!result.IsSuccess)
            TempData["Error"] = "Failed to upload image";

        return RedirectToAction("Index");
    }
}