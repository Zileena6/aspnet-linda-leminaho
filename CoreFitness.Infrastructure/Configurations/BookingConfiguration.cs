using CoreFitness.Domain.Entities.Bookings;
using CoreFitness.Domain.Entities.Bookings.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitness.Infrastructure.Configurations;

public class BookingConfiguration : BaseEntityConfiguration<Booking, BookingId>
{
    public override void Configure(EntityTypeBuilder<Booking> builder)
    {
        base.Configure(builder);

        builder.Property(b => b.UserId).IsRequired();

        builder.HasIndex(b => new { b.UserId, b.TrainingSessionId }).IsUnique();
    }
}