using CoreFitness.Domain.Entities.Memberships;
using CoreFitness.Domain.Entities.Memberships.ValueObjects;
using CoreFitness.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitness.Infrastructure.Configurations;

public class MembershipTypeConfiguration : BaseEntityConfiguration<MembershipType, MembershipTypeId>
{
    public override void Configure(EntityTypeBuilder<MembershipType> builder)
    {
        base.Configure(builder);

        builder.Property(t => t.Name)
            .HasMaxLength(MembershipTypeName.MaxLength)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasMaxLength(MembershipTypeDescription.MaxLength)
            .IsRequired();

        builder.Property(t => t.Price)
            .HasColumnType("decimal(10,2)");

        builder.Property(t => t.Duration);

        builder.Property(t => t.Type)
            .HasConversion<string>()
            .HasMaxLength(TypeConstants.MaxLength)
            .IsRequired();

        builder.Property(t => t.SessionLimit).IsRequired();

        builder.HasMany(t => t.Benefits)
            .WithOne()
            .HasForeignKey(b => b.MembershipTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(t => t.Benefits)
            .HasField("_benefits")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}