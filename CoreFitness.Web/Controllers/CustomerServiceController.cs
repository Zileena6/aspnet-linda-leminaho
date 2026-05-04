using CoreFitness.Web.ViewModels.CustomerService;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Web.Controllers;

public class CustomerServiceController : Controller
{
    public IActionResult Index()
    {
        return View(new ContactUsViewModel());
    }

    [HttpPost]
    public IActionResult ContactUs(ContactUsViewModel vm)
    {
        if (!ModelState.IsValid)
            return View("Index", vm);

        TempData["Success"] = "Message sent successfully!";

        return RedirectToAction(nameof(Index));
    }
}