using CoreFitness.Application.DTOs.TrainingSession;
using CoreFitness.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Web.Controllers;

[Authorize]
public class TrainingSessionsController(ITrainingSessionService trainingSessionService) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct = default)
    {
        var result = await trainingSessionService.GetUpcomingAsync(ct);

        if(!result.IsSuccess)
        {
            TempData["Error"] = result.Error;
            return View(new List<TrainingSessionDTO>());
        }

        return View(result.Value);
    }
}
