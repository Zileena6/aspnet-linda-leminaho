using CoreFitness.Application.DTOs.TrainingSession;
using CoreFitness.Application.Interfaces;
using CoreFitness.web.ViewModels.Admin.Sessions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.web.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("Admin/Sessions")]
public class AdminSessionsController(ITrainingSessionService trainingSessionService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var result = await trainingSessionService.GetUpcomingAsync();

        var vm = new AdminSessionsViewModel
        {
            Sessions = result.IsSuccess ? result.Value : []
        };

        return View(vm);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateSessionViewModel vm)
    {
        if (!ModelState.IsValid)
            return RedirectToAction(nameof(Index));

        var startDate = DateTime.SpecifyKind(vm.StartDate, DateTimeKind.Local);
        var startDateOffset = new DateTimeOffset(startDate);

        var dto = new CreateTrainingSessionDTO
        {
            Name = vm.Name,
            Description = vm.Description,
            Capacity = vm.Capacity,
            StartDate = startDateOffset,
            DurationInMinutes = vm.DurationInMinutes
        };

        var result = await trainingSessionService.CreateAsync(dto);

        if (!result.IsSuccess)
            TempData["Error"] = result.Error!.Message;

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("Delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await trainingSessionService.DeleteAsync(id);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await trainingSessionService.GetByIdAsync(id);

        if (!result.IsSuccess)
            return RedirectToAction(nameof(Index));

        var session = result.Value;

        var vm = new UpdateSessionViewModel
        {
            Id = session.Id,
            Name = session.Name,
            Description = session.Description,
            StartDate = session.StartDate.DateTime,
            Capacity = session.Capacity,
            DurationInMinutes = session.DurationInMinutes
        };

        return View(vm);
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update(UpdateSessionViewModel vm)
    {
        if (!ModelState.IsValid)
            return View("EditSession", vm);

        var startDate = DateTime.SpecifyKind(vm.StartDate, DateTimeKind.Local);
        var startDateOffset = new DateTimeOffset(startDate);

        var dto = new UpdateTrainingSessionDTO
        {
            Id = vm.Id,
            Name = vm.Name,
            Description = vm.Description,
            StartDate = startDateOffset,
            Capacity = vm.Capacity,
            DurationInMinutes = vm.DurationInMinutes
        };

        var result = await trainingSessionService.UpdateAsync(dto);

        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Error!.Message;
            return View("Edit", vm);
        }
        ;

        return RedirectToAction(nameof(Index));
    }
}
