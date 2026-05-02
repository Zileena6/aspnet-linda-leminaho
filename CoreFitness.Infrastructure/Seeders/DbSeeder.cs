using CoreFitness.Domain.Entities.Memberships;
using CoreFitness.Domain.Entities.Memberships.ValueObjects;
using CoreFitness.Domain.Enums;
using CoreFitness.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CoreFitness.Domain.Interfaces.Users;
using CoreFitness.Domain.Interfaces.UnitOfWork;
using CoreFitness.Domain.Entities.Users.ValueObjects;
using CoreFitness.Domain.Entities.Users;
using CoreFitness.Domain.Entities.TrainingSessions;
using CoreFitness.Domain.Entities.TrainingSessions.ValueObjects;

namespace CoreFitness.Infrastructure.Seeders;

public static class DbSeeder
{
    public static async Task SeedRolesAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        foreach (var role in new[] { "Member", "Admin", "Trainer" })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }
    }

    public static async Task SeedAdminAsync(IServiceProvider services)
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var userRepository = services.GetRequiredService<IUserRepository>();
        var unitOfWork = services.GetRequiredService<IUnitOfWork>();

        var adminEmail = "admin@core.com";
        var adminPassword = "Admin12!";

        var identityUser = await userManager.FindByEmailAsync(adminEmail);

        if (identityUser is null)
        {
            identityUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(identityUser, adminPassword);

            if (!result.Succeeded)
                throw new Exception("Failed to create admin user");
        }

        if (!await userManager.IsInRoleAsync(identityUser, "Admin"))
        {
            await userManager.AddToRoleAsync(identityUser, "Admin");
        }

        var authId = AuthenticationId.Create(identityUser.Id.ToString());

        var existiongUser = await userRepository.GetByAuthenticationIdAsync(authId);

        if (existiongUser is null)
        {
            var domainUser = User.Create(
                authId,
                UserEmail.Create(adminEmail),
                UserName.Create("Admin", "User"),
                null,
                null,
                UserRole.Admin
            );

            await userRepository.AddAsync(domainUser);

            await unitOfWork.SaveChangesAsync();
        }
    }

    public static async Task SeedMembershipTypesAsync(IServiceProvider services)
    {
        var context = services.GetRequiredService<CoreFitnessDbContext>();

        if (await context.MembershipTypes.AnyAsync()) return;

        var trial = MembershipType.Create(
            MembershipTypeName.Create("Trial"),
            MembershipTypeDescription.Create("A 7 day trial membership to try out our amazing gym!"),
            MembershipTypePrice.Create(0),
            MembershipTypeDuration.Create(7),
            5
            );

        trial.AddBenefit(MembershipTypeBenefitDescription.Create("Access to our premium locker"));
        trial.AddBenefit(MembershipTypeBenefitDescription.Create("High-energyt fitness class"));
        trial.AddBenefit(MembershipTypeBenefitDescription.Create("Motivating & supportive environment"));

        var standard = MembershipType.Create(
            MembershipTypeName.Create("Standard"),
            MembershipTypeDescription.Create("With the Standard Membership, get access to our full range of gym facilities"),
            MembershipTypePrice.Create(495),
            MembershipTypeDuration.Create(30),
            20
            );

        standard.AddBenefit(MembershipTypeBenefitDescription.Create("Standard locker"));
        standard.AddBenefit(MembershipTypeBenefitDescription.Create("High-energy group fitness classes"));
        standard.AddBenefit(MembershipTypeBenefitDescription.Create("Motivating & supportive environment"));

        var premium = MembershipType.Create(
            MembershipTypeName.Create("Premium"),
            MembershipTypeDescription.Create("With the Premium Membership, get access to our full range of gym facilities"),
            MembershipTypePrice.Create(595),
            MembershipTypeDuration.Create(30),
            20
            );

        premium.AddBenefit(MembershipTypeBenefitDescription.Create("Priority Support & Premium locker"));
        premium.AddBenefit(MembershipTypeBenefitDescription.Create("High-energy group fitness classes"));
        premium.AddBenefit(MembershipTypeBenefitDescription.Create("Motivating & supportive environment"));

        await context.MembershipTypes.AddRangeAsync(trial, standard, premium);
        await context.SaveChangesAsync();
    }

    public static async Task SeedTrainingSessions(IServiceProvider services)
    {
        var context = services.GetRequiredService<CoreFitnessDbContext>();

        if (await context.TrainingSessions.AnyAsync()) return;

        var now = DateTimeOffset.UtcNow;

        var sessions = new[]
            {
            TrainingSession.Create(
                TrainingSessionName.Create("Yoga"),
                TrainingSessionDescription.Create("Relax and take a deep breath"),
                now.AddDays(5),
                TrainingSessionCapacity.Create(40),
                TrainingSessionDuration.FromMinutes(60)
            ),
            TrainingSession.Create(
                TrainingSessionName.Create("Core fitness"),
                TrainingSessionDescription.Create("Spinn wheels and do rope waves"),
                now.AddDays(2),
                TrainingSessionCapacity.Create(20),
                TrainingSessionDuration.FromMinutes(30)
            ),
        };

        await context.TrainingSessions.AddRangeAsync(sessions);
        await context.SaveChangesAsync();
    }
}