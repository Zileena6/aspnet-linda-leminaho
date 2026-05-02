namespace CoreFitness.web.ViewModels.Auth;

public class ConfirmExternalAccountViewModel
{
    public string Email { get; set; } = string.Empty;
    public string? ReturnUrl { get; set; }
}
