using CoreFitness.Application.DTOs.User;

namespace CoreFitness.Web.ViewModels.Admin.Users;

public class AdminUsersViewModel
{
    public IEnumerable<AdminUserDTO> Users { get; set; } = [];
}