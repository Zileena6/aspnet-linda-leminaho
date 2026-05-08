using CoreFitness.Application.Authentication;
using CoreFitness.Web.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Web.ViewComponents;

    public class ExternalProvidersViewComponent(IAuthService authService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string? returnUrl = null)
        {
            var providers = await authService.GetExternalProvidersAsync();

            return View(new ExternalProvidersViewModel
            {
                Providers = providers,
                ReturnUrl = returnUrl
            });
        }
    }