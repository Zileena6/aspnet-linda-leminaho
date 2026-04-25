using CoreFitness.Domain.Entities.TrainingSessions;
using CoreFitness.Domain.Entities.TrainingSessions.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitness.Infrastructure.Configurations;

public class TrainingSessionConfiguration : BaseEntityConfiguration<TrainingSession, TrainingSessionId>
{
    public override void Configure(EntityTypeBuilder<TrainingSession> builder)
    {
        base.Configure(builder);

        builder.Property(ts => ts.TrainingSessionName)
            .HasColumnName("Name")
            .HasMaxLength(TrainingSessionName.MaxLength)
            .IsRequired();

        builder.Property(ts => ts.TrainingSessionDescription)
            .HasColumnName("Description")
            .HasMaxLength(TrainingSessionDescription.MaxLength)
            .IsRequired();

        builder.Property(ts => ts.Capacity)
            .HasColumnName("Capacity")
            .IsRequired();

        builder.Property(ts => ts.Duration)
            .HasColumnName("Duration")
            .IsRequired();

        builder.Property(ts => ts.StartDate)
            .IsRequired();

        builder.Ignore(ts => ts.EndDate);
        builder.Ignore(ts => ts.IsFull);

        builder.HasMany(b => b.Bookings)
            .WithOne(t => t.TrainingSession)
            .HasForeignKey(t => t.TrainingSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(ts => ts.Bookings)
            .HasField("_bookings")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}