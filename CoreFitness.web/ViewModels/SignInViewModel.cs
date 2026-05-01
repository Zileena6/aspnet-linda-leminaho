namespace CoreFitness.web.ViewModels;

public class SignInViewModel
{
    public string? ReturnUrl { get; set; }
    public List<string> ExternalProviders { get; set; } = [];
}
