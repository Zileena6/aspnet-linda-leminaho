using CoreFitness.Domain.Entities.Bookings.ValueObjects;
using CoreFitness.Domain.Entities.Memberships.ValueObjects;
using CoreFitness.Domain.Entities.TrainingSessions.ValueObjects;
using CoreFitness.Domain.Entities.Users.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoreFitness.Infrastructure.Converters;

public class UserIdConverter() : ValueConverter<UserId, Guid>(id => id.Value, v => new UserId(v));
public class AuthenticationIdConverter() : ValueConverter<AuthenticationId, string>(id => id.Value, v => AuthenticationId.Create(v));
public class MembershipIdConverter() : ValueConverter<MembershipId, Guid>(id => id.Value, v => new MembershipId(v));
public class MembershipTypeIdConverter() : ValueConverter<MembershipTypeId, Guid>(id => id.Value, v => new MembershipTypeId(v));
public class BookingIdConverter() : ValueConverter<BookingId, Guid>(id => id.Value, v => new BookingId(v));
public class TrainingSessionIdConverter() : ValueConverter<TrainingSessionId, Guid>(id => id.Value, v => new TrainingSessionId(v));
public class CheckInIdConverter() : ValueConverter<CheckInId, Guid>(id => id.Value, v => new CheckInId(v));
public class MembershipTypeBenefitIdConverter() : ValueConverter<MembershipTypeBenefitId, Guid>(id => id.Value, v => new MembershipTypeBenefitId(v));