using CoreFitness.Application.DTOs.Membership;
using CoreFitness.Application.Interfaces;
using CoreFitness.web.ViewModels.Admin.Memberships;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.web.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("Admin/Memberships")]
public class AdminMembershipsController(IMembershipService membershipService, IMembershipTypeService membershipTypeService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var result = await membershipTypeService.GetAllAsync();

        var vm = new AdminMembershipsViewModel
        {
            MembershipTypes = result.IsSuccess ? result.Value : []
        };

        return View(vm);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(MembershipTypeFormViewModel vm)
    {
        if (!ModelState.IsValid)
            return RedirectToAction(nameof(Index));

        var dto = new CreateMembershipTypeDTO
        {
            Name = vm.Name,
            Description = vm.Description,
            Price = vm.Price,
            DurationInDays = vm.DurationInDays,
            SessionLimit = vm.SessionLimit
        };

        var result = await membershipTypeService.CreateAsync(dto);

        if (!result.IsSuccess)
            TempData["Error"] = result.Error!.Message;

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("Delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await membershipTypeService.DeleteAsync(id);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await membershipTypeService.GetByIdAsync(id);

        if (!result.IsSuccess)
            return RedirectToAction(nameof(Index));

        var types = await membershipTypeService.GetAllAsync();

        var type = result.Value;

        var vm = new AdminMembershipsViewModel
        {
            MembershipTypes = types.IsSuccess ? types.Value : [],
            UpdateMembershipType = new MembershipTypeFormViewModel
            {
                Id = type.Id,
                Name = type.Name,
                Description = type.Description,
                Price = type.Price,
                DurationInDays = type.DurationInDays,
                SessionLimit = type.SessionLimit
            }
        };

        return View("Index", vm);
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update(MembershipTypeFormViewModel vm)
    {
        if (!ModelState.IsValid)
            return RedirectToAction(nameof(Index));

        var dto = new UpdateMembershipTypeDTO
        {
            Id = vm.Id!.Value,
            Name = vm.Name,
            Description = vm.Description,
            Price = vm.Price,
            DurationInDays = vm.DurationInDays,
            SessionLimit = vm.SessionLimit
        };

        var result = await membershipTypeService.UpdateAsync(dto);

        if (!result.IsSuccess)
            TempData["Error"] = result.Error!.Message;

        return RedirectToAction(nameof(Index));
    }
}