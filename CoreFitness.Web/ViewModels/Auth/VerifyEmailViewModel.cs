using System.ComponentModel.DataAnnotations;

namespace CoreFitness.Web.ViewModels.Auth;

public class VerifyEmailViewModel
{
    public string Email { get; set; } = string.Empty;
    public string? ReturnUrl { get; set; }
    [Required(ErrorMessage = "Please provide verification code")]
    public string Code { get; set; } = string.Empty;
}