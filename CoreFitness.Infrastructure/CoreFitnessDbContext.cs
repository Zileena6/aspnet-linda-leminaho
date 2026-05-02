using CoreFitness.Domain.Entities.Bookings;
using CoreFitness.Domain.Entities.Bookings.ValueObjects;
using CoreFitness.Domain.Entities.Memberships;
using CoreFitness.Domain.Entities.Memberships.ValueObjects;
using CoreFitness.Domain.Entities.TrainingSessions;
using CoreFitness.Domain.Entities.TrainingSessions.ValueObjects;
using CoreFitness.Domain.Entities.Users;
using CoreFitness.Domain.Entities.Users.ValueObjects;
using CoreFitness.Infrastructure.Converters;
using CoreFitness.Infrastructure.Primitives;
using Microsoft.EntityFrameworkCore;
using static CoreFitness.Infrastructure.Converters.ValueObjectConverters;

namespace CoreFitness.Infrastructure;

public class CoreFitnessDbContext(DbContextOptions<CoreFitnessDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Membership> Memberships { get; set; }
    public DbSet<MembershipType> MembershipTypes { get; set; }
    public DbSet<TrainingSession> TrainingSessions { get; set; }
    public DbSet<MembershipTypeBenefit> MembershipTypeBenefits { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<UserId>().HaveConversion<UserIdConverter>();
        configurationBuilder.Properties<AuthenticationId>().HaveConversion<AuthenticationIdConverter>();
        configurationBuilder.Properties<MembershipId>().HaveConversion<MembershipIdConverter>();
        configurationBuilder.Properties<MembershipTypeId>().HaveConversion<MembershipTypeIdConverter>();
        configurationBuilder.Properties<BookingId>().HaveConversion<BookingIdConverter>();
        configurationBuilder.Properties<TrainingSessionId>().HaveConversion<TrainingSessionIdConverter>();
        configurationBuilder.Properties<CheckInId>().HaveConversion<CheckInIdConverter>();
        configurationBuilder.Properties<MembershipTypeBenefitId>().HaveConversion<MembershipTypeBenefitIdConverter>();

        configurationBuilder.Properties<MembershipTypeBenefitDescription>().HaveConversion<MembershipTypeBenefitDescriptionConverter>();
        configurationBuilder.Properties<MembershipTypeDescription>().HaveConversion<MembershipTypeDescriptionConverter>();
        configurationBuilder.Properties<MembershipTypeDuration>().HaveConversion<MembershipTypeDurationConverter>();
        configurationBuilder.Properties<MembershipTypeName>().HaveConversion<MembershipTypeNameConverter>();
        configurationBuilder.Properties<MembershipTypePrice>().HaveConversion<MembershipTypePriceConverter>();
        configurationBuilder.Properties<TrainingSessionCapacity>().HaveConversion<TrainingSessionCapacityConverter>();
        configurationBuilder.Properties<TrainingSessionName>().HaveConversion<TrainingSessionNameConverter>();
        configurationBuilder.Properties<TrainingSessionDescription>().HaveConversion<TrainingsessionDescriptionConverter>();
        configurationBuilder.Properties<TrainingSessionDuration>().HaveConversion<TrainingSessionDurationConverter>();
        configurationBuilder.Properties<UserEmail>().HaveConversion<UserEmailConverter>();
        configurationBuilder.Properties<UserPhoneNumber>().HaveConversion<UserPhoneNumberConverter>();

        base.ConfigureConventions(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IntResult>().HasNoKey();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoreFitnessDbContext).Assembly);

        if (Database.IsSqlite())
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var rowVersion = entity.FindProperty("RowVersion");
                rowVersion?.SetDefaultValueSql("randomblob(8)");
            }
        }

        base.OnModelCreating(modelBuilder);
    }
}
