using System.ComponentModel.DataAnnotations;

namespace CoreFitness.web.ViewModels.Auth;

public class SignUpViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    public IReadOnlyList<string> ExternalProviders { get; set; } = [];
}