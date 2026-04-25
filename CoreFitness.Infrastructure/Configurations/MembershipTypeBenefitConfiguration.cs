using CoreFitness.Domain.Entities.Memberships;
using CoreFitness.Domain.Entities.Memberships.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitness.Infrastructure.Configurations;

public class MembershipTypeBenefitConfiguration : BaseEntityConfiguration<MembershipTypeBenefit, MembershipTypeBenefitId>
{
    public override void Configure(EntityTypeBuilder<MembershipTypeBenefit> builder)
    {
        base.Configure(builder);

        builder.Property(b => b.Description)
            .HasMaxLength(MembershipTypeBenefitDescription.MaxLength)
            .IsRequired();

        builder.Property(b => b.MembershipTypeId).IsRequired();
    }
}