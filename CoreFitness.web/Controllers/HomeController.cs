using CoreFitness.Application.Interfaces;
using CoreFitness.web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CoreFitness.web.Controllers;

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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Route("Home/Error404")]
    public IActionResult Error404() => View();
}
