using System.ComponentModel.DataAnnotations;

namespace CoreFitness.Web.ViewModels.Auth;

public class SignUpViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}