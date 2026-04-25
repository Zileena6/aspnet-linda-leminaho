using CoreFitness.Domain.Entities.Memberships;
using CoreFitness.Domain.Entities.Memberships.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitness.Infrastructure.Configurations;

public class MembershipConfiguration : BaseEntityConfiguration<Membership, MembershipId>
{
    public override void Configure(EntityTypeBuilder<Membership> builder)
    {
        base.Configure(builder);

        builder.HasOne<MembershipType>()
            .WithMany()
            .HasForeignKey(m => m.TypeId);

        builder.Property(m => m.StartDate)
            .HasConversion(d => d.ToDateTime(TimeOnly.MinValue),
            d => DateOnly.FromDateTime(d))
            .HasColumnType("date")
            .IsRequired();

        builder.Property(m => m.EndDate)
            .HasConversion(d => d.ToDateTime(TimeOnly.MinValue),
            d => DateOnly.FromDateTime(d))
            .HasColumnType("date")
            .IsRequired();

        builder.HasMany(m => m.CheckIns)
            .WithOne()
            .HasForeignKey(c => c.MembershipId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(m => m.CheckIns)
            .HasField("_checkIns")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(m => m.IsActive);

        builder.Ignore(m => m.IsExpired);

        builder.Ignore(m => m.HasSessionsLeft);

        builder.Ignore(m => m.CheckInsLast30Days);

        builder.Property(m => m.IsManuallyDeactivated).IsRequired();

        builder.Property(m => m.SessionsUsed).IsRequired();

        builder.Property(m => m.SessionLimit).IsRequired();
    }
}