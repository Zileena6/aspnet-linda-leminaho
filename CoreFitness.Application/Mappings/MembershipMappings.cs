using CoreFitness.Application.DTOs.Membership;
using CoreFitness.Domain.Entities.Memberships;

namespace CoreFitness.Application.Mappings;

public static class MembershipMappings
{
    public static MembershipDTO ToDTO(
        this Membership membership, 
        string membershipTypeName, 
        decimal price) => new()
    {
        Id = membership.Id.Value,
        MembershipTypeName = membershipTypeName,
        PurchasedPrice = membership.PurchasedPrice,
        StartDate = membership.StartDate,
        EndDate = membership.EndDate,
        IsActive = membership.IsActive,
        IsExpired = membership.IsExpired,
        IsManuallyDeactivated = membership.IsManuallyDeactivated,
        SessionsUsed = membership.SessionsUsed,
        SessionLimit = membership.SessionLimit,
        CheckInsLast30Days = membership.CheckInsLast30Days
    };

    public static MembershipTypeDTO ToDTO(this MembershipType type) => new()
    {
        Id = type.Id.Value,
        Name = type.Name.Value,
        Description = type.Description.Value,
        Price = type.Price.Value,
        DurationInDays = type.Duration.Value,
        SessionLimit = type.SessionLimit,
        Benefits = [.. type.Benefits.Select(b => b.Description.Value)]
    };

    public static CheckInDTO ToDTO(this CheckIn checkIn) => new()
    {
        Id = checkIn.Id.Value,
        CheckedInAt = checkIn.CheckedInAt
    };
}