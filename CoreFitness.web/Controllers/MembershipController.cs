using CoreFitness.Application.Interfaces;
using CoreFitness.web.ViewModels.Membership;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.web.Controllers;

[Route("memberships")]
public class MembershipController(IMembershipService membershipService, IFaqService faqService) : Controller
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
}