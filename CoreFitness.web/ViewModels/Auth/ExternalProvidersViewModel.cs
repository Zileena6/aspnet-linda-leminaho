namespace CoreFitness.Web.ViewModels.Auth;

    public class ExternalProvidersViewModel
    {
        public IReadOnlyList<string> Providers { get; init; } = [];
        public string? ReturnUrl { get; init; }
    }