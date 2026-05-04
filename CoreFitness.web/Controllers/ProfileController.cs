using CoreFitness.Application.Authentication;
using CoreFitness.Application.DTOs.User;
using CoreFitness.Application.Interfaces;
using CoreFitness.Web.Extensions;
using CoreFitness.Web.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Web.Controllers;

[Authorize]
public class ProfileController(
    IUserService userService, 
    IAuthService authService,
    ITrainingSessionService trainingSessionService,
    IMembershipService membershipService,
    ILogger<ProfileController> logger) : Controller
{
    public async Task<IActionResult> Index(ProfileTabs tab = ProfileTabs.About, CancellationToken ct = default)
    {
        var authId = User.GetAuthenticationId();

        var userResult = await userService.GetByAuthenticationId(authId, ct);

        if(!userResult.IsSuccess)
        {
            await authService.SignOutAsync(ct);

            TempData["Error"] = "Your account was not found. Please sign in again or create a new account";
            
            return RedirectToAction("SignIn", "Account");
        }

        var user = userResult.Value!;

        var statsResult = await userService.GetStatisticsAsync(userResult.Value!.Id, ct);
        var membershipResult = await membershipService.GetByUserIdAsync(authId, ct);

        var vm = new ProfilePageViewModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            PhotoUrl = user.PhotoUrl,
            Statistics = statsResult.IsSuccess ? statsResult.Value : null,
            Weight = statsResult.IsSuccess ? statsResult.Value?.CurrentWeight : null,
            Height = statsResult.IsSuccess ? statsResult.Value?.Height : null,
            TargetWeight = statsResult.IsSuccess ? statsResult.Value?.TargetWeight : null,

            Membership = membershipResult.IsSuccess ? membershipResult.Value : null,
            
            ActiveTab = tab,

            UpdateForm = new UpdateProfileViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Weight = statsResult.IsSuccess ? statsResult.Value?.CurrentWeight : null,
                Height = statsResult.IsSuccess ? statsResult.Value?.Height : null,
                TargetWeight = statsResult.IsSuccess ? statsResult.Value?.TargetWeight : null,
                RowVersion = user.RowVersion
            }
        };

        if(tab == ProfileTabs.Bookings)
        {            
            var bookingsResult = await trainingSessionService.GetUserBookingsAsync(user.Id, ct);

            vm.Bookings = bookingsResult.IsSuccess ? bookingsResult.Value : [];
        }

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateProfileViewModel vm, CancellationToken ct = default)
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
            RowVersion = vm.RowVersion
        };

        var result = await userService.UpdateProfileAsync(dto, ct);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result?.Error?.Message ?? "Update failed");

            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = "Profile updated successfully";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
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

    [HttpPost]
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