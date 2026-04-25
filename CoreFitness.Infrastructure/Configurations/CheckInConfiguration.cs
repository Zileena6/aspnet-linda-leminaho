using CoreFitness.Domain.Entities.Memberships;
using CoreFitness.Domain.Entities.Memberships.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitness.Infrastructure.Configurations;

public class CheckInConfiguration : BaseEntityConfiguration<CheckIn, CheckInId>
{
    public override void Configure(EntityTypeBuilder<CheckIn> builder)
    {
        base.Configure(builder);

        builder.Property(c => c.UserId).IsRequired();

        builder.Property(c => c.MembershipId).IsRequired();

        builder.Property(c => c.CheckedInAt).IsRequired();
    }
}