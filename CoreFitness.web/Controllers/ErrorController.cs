using CoreFitness.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Web.Controllers;

// [Route("Error")]
[AllowAnonymous]
public class ErrorController : Controller
{
    [HttpGet]
    public IActionResult Index(int statusCode)
    {
        var message = statusCode switch
        {
            404 => "Oops! Page Not Found!",
            400 => "Invalid requres",
            409 => "Conflict occured",
            422 => "Business rule violation",
            _ => "Something went wrong"
        };

        var vm = new ErrorViewModel
        {
            StatusCode = statusCode,
            Message = message
        };

        return View(vm);
    }
}
