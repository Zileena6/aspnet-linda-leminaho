using CoreFitness.Application.Interfaces;
using CoreFitness.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Web.Controllers;

[Authorize]
public class BookingController(ITrainingSessionService trainingSessionService) : Controller
{
    [HttpPost]
    public async Task<IActionResult> Book(Guid sessionId, CancellationToken ct = default)
    {
        var userId = User.GetAuthenticationId();

        var result = await trainingSessionService.BookAsync(sessionId, userId, ct);
        Console.WriteLine($"BookAsync result: {result.IsSuccess}, Error: {result.Error?.Message}");


        if(!result.IsSuccess)
        {
            TempData["Error"] = result.Error?.Message;
            return RedirectToAction("Index", "TrainingSessions");
        }

        TempData["Success"] = "You are booked!";
        return RedirectToAction("Index", "TrainingSessions");
    }

    [HttpPost]
    public async Task<IActionResult> Cancel(Guid sessionId, CancellationToken ct = default)
    {
        var userId = User.GetAuthenticationId();

        var result = await trainingSessionService.CancelBookingAsync(sessionId, userId, ct);

        if(!result.IsSuccess)
        {
            TempData["Error"] = result.Error;
        }

        return RedirectToAction("Index", "Profile");
    }
}
