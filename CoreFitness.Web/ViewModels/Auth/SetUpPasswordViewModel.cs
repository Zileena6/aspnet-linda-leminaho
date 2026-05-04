using System.ComponentModel.DataAnnotations;

namespace CoreFitness.Web.ViewModels.Auth;

public class SetUpPasswordViewModel
{
    public string Email { get; set; } = string.Empty;
    public string? ReturnUrl { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Passwoord must be at least 8 characters")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please confirm your password")]
    [Compare(nameof(Password), ErrorMessage = "Passwords does not match")]
    public string ConfirmPassword { get; set; } = string.Empty;
}