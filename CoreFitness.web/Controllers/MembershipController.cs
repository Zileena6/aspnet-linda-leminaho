using CoreFitness.Application.DTOs.Membership;
using CoreFitness.Application.Interfaces;
using CoreFitness.Web.Extensions;
using CoreFitness.Web.ViewModels.Membership;
using CoreFitness.Web.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Web.Controllers;

    [Route("memberships")]
    public class MembershipController(IMembershipService membershipService,IUserService userService, IFaqService faqService) : Controller
    {
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var membershipResult = await membershipService.GetMembershipTypesAsync();
            var faqItems = await faqService.GetFaqItemsAsync();

            var viewModel = new MembershipPageViewModel
            {
                MembershipTypes = membershipResult.IsSuccess ? membershipResult.Value! : [],
                FaqItems = faqItems
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost("Join")]
        public async Task<IActionResult> Join(Guid membershipTypeId, CancellationToken ct = default)
        {
            var authId = User.GetAuthenticationId();

            var dto = new CreateMembershipDTO
            {
                MembershipTypeId = membershipTypeId
            };

            var result = await membershipService.CreateAsync(authId, dto, ct);

            if(!result.IsSuccess)
            {
                TempData["Error"] = result.Error?.Message ?? "Could not join membership.";

                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Activation successful";

            return RedirectToAction("Index", "Profile", new { tab = ProfileTabs.Membership});
        }

// TODO: Is Already deactivated! If active bookings - Cancel!!
        [Authorize]
        [HttpPost("Cancel")]
        public async Task<IActionResult> Cancel(CancellationToken ct = default)
        {
            var authId = User.GetAuthenticationId();

            var result = await membershipService.DeactivateAsync(authId, ct);

            if(!result.IsSuccess)
            {
                TempData["Error"] = result.Error?.Message ?? "Could not cancel membership";
                return RedirectToAction("Index", "Profile");
            }

            TempData["Success"] = "Membeship cancelled";
            return RedirectToAction("Index", "Profile", new { tab = ProfileTabs.Bookings});
        }

        [Authorize]
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(CancellationToken ct = default)
        {
            var authId = User.GetAuthenticationId();

            var result = await membershipService.DeleteAsync(authId, ct);

            if(!result.IsSuccess)
            {
                TempData["Error"] = result.Error?.Message ?? "Could not delete membeship";
                return RedirectToAction("Index", "Profile");
            }

            TempData["Success"] = "Membership deleted";
            return RedirectToAction("Index", "Profile");
        }
    }