using CoreFitness.Application.Interfaces;
using CoreFitness.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CoreFitness.Web.Controllers;

public class HomeController(IQuoteService quoteService) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> GetQuote()
    {
        var quote = await quoteService.GetRandomQuoteAsync();
        if (quote is null)
            return StatusCode(500);

        return Json(quote);
    }
}
