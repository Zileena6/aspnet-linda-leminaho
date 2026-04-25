using CoreFitness.Domain.Entities.Users;
using CoreFitness.Domain.Entities.Users.ValueObjects;
using CoreFitness.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static CoreFitness.Infrastructure.Converters.ValueObjectConverters;

namespace CoreFitness.Infrastructure.Configurations;

public class UserConfiguration : BaseEntityConfiguration<User, UserId>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.Property(u => u.AuthenticationId)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasMaxLength(254)
            .IsRequired();

        builder.Property(u => u.EmailUnique)
            .HasMaxLength(254)
            .IsRequired();

        builder.HasIndex(u => u.EmailUnique).IsUnique();

        builder.ComplexProperty(u => u.UserName, name =>
        {
            name.Property(n => n.FirstName)
            .HasColumnName("FirstName")
            .HasMaxLength(UserName.MaxLength)
            .IsRequired();

            name.Property(n => n.LastName)
            .HasColumnName("LastName")
            .HasMaxLength(UserName.MaxLength)
            .IsRequired();
        });

        builder.Property(u => u.UserPhoneNumber)
            .HasConversion<UserPhoneNumberConverter>();

        builder.Property(u => u.Role)
            .HasConversion<string>()
            .HasMaxLength(UserRoleConstants.MaxLength)
            .IsRequired();

        builder.Property(u => u.PhotoUrl)
            .HasMaxLength(500);

        builder.Property(m => m.CurrentWeight)
            .HasColumnType("decimal(5,2)");

        builder.Property(m => m.TargetWeight)
            .HasColumnType("decimal(5,2)");

        builder.Property(m => m.Height)
            .HasColumnType("decimal(5,2)");

        builder.Ignore(m => m.BMI);
    }
}