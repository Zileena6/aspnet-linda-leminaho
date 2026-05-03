using System.ComponentModel.DataAnnotations;

namespace CoreFitness.Web.ViewModels.Auth;

public class VerifyExternalLogInViewModel
{
    public string Email { get; set; } = null!;
    public string? ReturnUrl { get; set; }

    [Required(ErrorMessage = "Please provide verification code")]
    public string Code { get; set; } = null!;
}
