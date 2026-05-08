using CoreFitness.Application.Interfaces;
using CoreFitness.Web.ViewModels.CustomerService;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Web.Controllers;

public class CustomerServiceController(IFaqService faqService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var faqItems = await faqService.GetFaqItemsAsync();

        var viewModel = new ContactUsViewModel
        {
            FaqItems = faqItems
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult ContactUs(ContactUsViewModel vm)
    {
        if(!ModelState.IsValid)
            return View("Index", vm);

        TempData["Success"] = "Message sent successfully!";

        return RedirectToAction(nameof(Index));
    }
}
