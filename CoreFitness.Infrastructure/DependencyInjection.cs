using CoreFitness.Application.Authentication;
using CoreFitness.Domain.Interfaces.Memberships;
using CoreFitness.Domain.Interfaces.TrainingSessions;
using CoreFitness.Domain.Interfaces.UnitOfWork;
using CoreFitness.Domain.Interfaces.Users;
using CoreFitness.Infrastructure.Identity;
using CoreFitness.Infrastructure.Persistence;
using CoreFitness.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreFitness.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration config,
        IHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            services.AddDbContext<CoreFitnessDbContext>(options =>
            options.UseSqlite("Data Source=corefitness-dev.db"));

            services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlite("Data Source=auth-dev.db"));
        }
        else
        {
            services.AddDbContext<CoreFitnessDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
        }

        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
        {
            options.SignIn.RequireConfirmedEmail = true;
        })
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication()
            .AddGoogle(options =>
            {
                var clientId = config["Authentication:Google:ClientId"];
                var clientSecret = config["Authentication:Google:ClientSecret"];

                if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
                {
                    throw new InvalidOperationException(
                        "Google authentication is missing in configuration." +
                        "Please set Authentication:Google:ClientId and ClientSecret");
                }

                options.ClientId = clientId;
                options.ClientSecret = clientSecret;

                options.CallbackPath = "/signin-google";
            });

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMembershipRepository, MembershipRepository>();
        services.AddScoped<IMembershipTypeRepository, MembershipTypeRepository>();
        services.AddScoped<ITrainingSessionRepository, TrainingSessionRepository>();

        return services;
    }
}