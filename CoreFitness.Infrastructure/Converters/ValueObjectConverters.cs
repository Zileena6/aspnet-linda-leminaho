using CoreFitness.Domain.Entities.Memberships.ValueObjects;
using CoreFitness.Domain.Entities.TrainingSessions.ValueObjects;
using CoreFitness.Domain.Entities.Users.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoreFitness.Infrastructure.Converters;

public class ValueObjectConverters
{
    public class MembershipTypeBenefitDescriptionConverter() : ValueConverter<MembershipTypeBenefitDescription, string>(
        v => v.Value,
        v => MembershipTypeBenefitDescription.Create(v)
        );

    public class MembershipTypeDescriptionConverter() : ValueConverter<MembershipTypeDescription, string>(
        v => v.Value,
        v => MembershipTypeDescription.Create(v)
        );

    public class MembershipTypeDurationConverter() : ValueConverter<MembershipTypeDuration, int>(
        v => v.Value,
        v => MembershipTypeDuration.Create(v)
        );

    public class MembershipTypeNameConverter() : ValueConverter<MembershipTypeName, string>(
        v => v.Value,
        v => MembershipTypeName.Create(v)
        );

    public class MembershipTypePriceConverter() : ValueConverter<MembershipTypePrice, decimal>(
        v => v.Value,
        v => MembershipTypePrice.Create(v)
        );

    public class TrainingSessionCapacityConverter() : ValueConverter<TrainingSessionCapacity, int>(
        v => v.Value,
        v => TrainingSessionCapacity.Create(v)
        );

    public class TrainingSessionNameConverter() : ValueConverter<TrainingSessionName, string>(
        v => v.Value,
        v => TrainingSessionName.Create(v)
        );

    public class TrainingsessionDescriptionConverter() : ValueConverter<TrainingSessionDescription, string>(
        v => v.Value,
        v => TrainingSessionDescription.Create(v)
        );

    public class TrainingSessionDurationConverter() : ValueConverter<TrainingSessionDuration, TimeSpan>(
        v => v.Value,
        v => TrainingSessionDuration.Create(v)
        );

    public class UserEmailConverter() : ValueConverter<UserEmail, string>(
        v => v.Value,
        v => UserEmail.Create(v)
        );

    public class UserPhoneNumberConverter() : ValueConverter<UserPhoneNumber?, string?>(
        v => v.HasValue ? v.Value.Value : null,
        v => v != null ? UserPhoneNumber.Create(v) : null
        );

    public class DateTimeOffsetConverter : ValueConverter<DateTimeOffset, DateTime>
    {
        public DateTimeOffsetConverter() : base(
            v => v.UtcDateTime,
            v => new DateTimeOffset(v, TimeSpan.Zero)
        )
        { }
    }
}