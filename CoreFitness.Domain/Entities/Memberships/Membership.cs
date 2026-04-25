using CoreFitness.Domain.Entities.Common;
using CoreFitness.Domain.Entities.Memberships.ValueObjects;
using CoreFitness.Domain.Entities.Users.ValueObjects;
using CoreFitness.Domain.Exceptions;
using CoreFitness.Domain.Interfaces;

namespace CoreFitness.Domain.Entities.Memberships;

public class Membership : BaseEntity<MembershipId>, IAggregateRoot
{
    private readonly List<CheckIn> _checkIns = [];
    public IReadOnlyCollection<CheckIn> CheckIns => _checkIns.AsReadOnly();
    public UserId UserId { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public MembershipTypeId TypeId { get; private set; }
    public bool IsExpired => DateOnly.FromDateTime(DateTime.UtcNow) > EndDate;
    public bool IsActive => !IsExpired && !IsManuallyDeactivated;
    public bool IsManuallyDeactivated { get; private set; }
    public int SessionsUsed { get; private set; }
    public int SessionLimit { get; private set; }
    public bool HasSessionsLeft => SessionsUsed < SessionLimit;

    private Membership(
        MembershipId id,
        UserId userId,
        MembershipTypeId type,
        DateOnly startDate,
        DateOnly endDate,
        int sessionLimit)
    {
        Id = id;
        UserId = userId;
        TypeId = type;
        StartDate = startDate;
        EndDate = endDate;
        SessionLimit = sessionLimit;
    }

    public static Membership Create(
        UserId userId, 
        MembershipTypeId typeId, 
        DateOnly startDate, 
        DateOnly endDate, 
        int sessionLimit) =>
            new(MembershipId.New(), userId, typeId, startDate, endDate, sessionLimit);

    private Membership() { }

    public void Extend(DateOnly newEndDate)
    {
        if (newEndDate <= EndDate)
            throw new InvalidExtendMembershipException("New date has to later than end date");

        EndDate = newEndDate;
    }

    public int CheckInsLast30Days => _checkIns
        .Count(c => c.CheckedInAt >= DateTimeOffset.UtcNow.AddDays(-30));

    public CheckIn RegisterCheckIn()
    {
        if (!IsActive)
            throw new MembershipExpiredException();

        var checkIn = CheckIn.Create(UserId, Id);
        _checkIns.Add(checkIn);
        UpdateTimeStamp();
        return checkIn;
    }

    public void UseSession()
    {
        if (!IsActive)
            throw new MembershipExpiredException();

        if (!HasSessionsLeft)
            throw new NoSessionsLeftException();

        SessionsUsed++;
        UpdateTimeStamp();
    }

    public void RefundSession()
    {
        if (SessionsUsed <= 0) return;

        SessionsUsed--;
        UpdateTimeStamp();
    }

    public void DeactivateMembership()
    {
        if (IsExpired)
            throw new MembershipExpiredException();

        if (IsManuallyDeactivated)
            throw new MembershipAlreadyDeactivatedException();

        IsManuallyDeactivated = true;
        UpdateTimeStamp();
    }

    public void ActivateMembership()
    {
        if (IsExpired)
            throw new MembershipExpiredException();

        if (!IsManuallyDeactivated) return;

        IsManuallyDeactivated = false;
        UpdateTimeStamp();
    }
}
