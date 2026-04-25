using CoreFitness.Domain.Entities.Memberships;
using CoreFitness.Domain.Entities.Memberships.ValueObjects;
using CoreFitness.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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

    public static async Task SeedMembershipTypesAsync(IServiceProvider services)
    {
        var context = services.GetRequiredService<CoreFitnessDbContext>();

        if (await context.MembershipTypes.AnyAsync()) return;

        var trial = MembershipType.Create(
            MembershipTypeName.Create("Trial"),
            MembershipTypeDescription.Create("A 7 day trial membership to try out our amazing gym!"),
            MembershipTypePrice.Create(0),
            MembershipTypeDuration.Create(7),
            5,
            MembershipTypeEnums.Trial
            );

        trial.AddBenefit(MembershipTypeBenefitDescription.Create("Access to our premium locker"));
        trial.AddBenefit(MembershipTypeBenefitDescription.Create("High-energy fitness class"));
        trial.AddBenefit(MembershipTypeBenefitDescription.Create("Motivating & supportive environment"));

        var standard = MembershipType.Create(
            MembershipTypeName.Create("Standard"),
            MembershipTypeDescription.Create("With the Standard Membership, you get access to our full range of gym facilities"),
            MembershipTypePrice.Create(495),
            MembershipTypeDuration.Create(30),
            20,
            MembershipTypeEnums.Standard
            );

        standard.AddBenefit(MembershipTypeBenefitDescription.Create("Standard locker"));
        standard.AddBenefit(MembershipTypeBenefitDescription.Create("High-energy group fitness classes"));
        standard.AddBenefit(MembershipTypeBenefitDescription.Create("Motivating & supportive environment"));

        var premium = MembershipType.Create(
            MembershipTypeName.Create("Premium"),
            MembershipTypeDescription.Create("With the Premium Membership, you get access to our full range of gym facilities"),
            MembershipTypePrice.Create(595),
            MembershipTypeDuration.Create(30),
            20,
            MembershipTypeEnums.Premium
            );

        premium.AddBenefit(MembershipTypeBenefitDescription.Create("Priority Support & Premium locker"));
        premium.AddBenefit(MembershipTypeBenefitDescription.Create("High-energy group fitness classes"));
        premium.AddBenefit(MembershipTypeBenefitDescription.Create("Motivating & supportive environment"));

        await context.MembershipTypes.AddRangeAsync(trial, standard, premium);
        await context.SaveChangesAsync();
    }
}