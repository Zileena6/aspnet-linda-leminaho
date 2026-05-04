using CoreFitness.Application.Interfaces;
using CoreFitness.Web.ViewModels.Admin.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Web.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("Admin/Users")]
public class AdminUsersController(IAdminService adminService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var result = await adminService.GetAllUsersAsync();

        var vm = new AdminUsersViewModel
        {
            Users = result.IsSuccess ? result.Value : []
        };

        return View(vm);
    }
}
