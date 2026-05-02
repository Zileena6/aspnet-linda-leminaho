using CoreFitness.Application.Interfaces;
using CoreFitness.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CoreFitness.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMembershipService, MembershipService>();
        services.AddScoped<ITrainingSessionService, TrainingSessionService>();
        services.AddHttpClient<IQuoteService, QuoteService>();
        services.AddScoped<IFaqService, FaqService>();

        return services;
    }
}
