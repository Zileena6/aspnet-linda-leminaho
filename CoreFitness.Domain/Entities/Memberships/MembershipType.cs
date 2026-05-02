using CoreFitness.Domain.Entities.Common;
using CoreFitness.Domain.Entities.Memberships.ValueObjects;
using CoreFitness.Domain.Exceptions;
using CoreFitness.Domain.Interfaces;

namespace CoreFitness.Domain.Entities.Memberships;

public class MembershipType : BaseEntity<MembershipTypeId>, IAggregateRoot
{
    public MembershipTypeName Name { get; private set; }
    public MembershipTypeDescription Description { get; private set; }
    public MembershipTypePrice Price { get; private set; }
    public MembershipTypeDuration Duration { get; private set; }
    public int SessionLimit { get; private set; }

    private readonly List<MembershipTypeBenefit> _benefits = [];
    public IReadOnlyCollection<MembershipTypeBenefit> Benefits => _benefits.AsReadOnly();

    public static MembershipType Create(
        MembershipTypeName name, 
        MembershipTypeDescription description, 
        MembershipTypePrice price, 
        MembershipTypeDuration duration, 
        int sessionLimit)
    {
        if (sessionLimit <= 0)
            throw new InvalidSessionLimitException(sessionLimit);

        return new(MembershipTypeId.New(), name, description, price, duration, sessionLimit, type);
    }

    private MembershipType(
        MembershipTypeId id, 
        MembershipTypeName name, 
        MembershipTypeDescription description, 
        MembershipTypePrice price, 
        MembershipTypeDuration duration, 
        int sessionLimit)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        Duration = duration;
        SessionLimit = sessionLimit;
    }

    protected MembershipType() { }

    public void UpdatePrice(MembershipTypePrice newPrice)
    {
        if (newPrice == Price) return;

        Price = newPrice;

        UpdateTimeStamp();
    }

    public void UpdateDuration(MembershipTypeDuration days)
    {
        Duration = days;

        UpdateTimeStamp();
    }

    public void UpdateSessionLimit(int newLimit)
    {
        if (newLimit <= 0)
            throw new InvalidSessionLimitException(newLimit);

        SessionLimit = newLimit;

        UpdateTimeStamp();
    }

    public void UpdateName(MembershipTypeName newName)
    {
        if (Name == newName) return;

        Name = newName;

        UpdateTimeStamp();
    }

    public void UpdateDescription(MembershipTypeDescription newDescription)
    {
        if (Description == newDescription) return;

        Description = newDescription;

        UpdateTimeStamp();
    }

    public void AddBenefit(MembershipTypeBenefitDescription description)
    {
        var benefit = MembershipTypeBenefit.Create(Id, description);

        _benefits.Add(benefit);

        UpdateTimeStamp();
    }

    public void RemoveBenefit(MembershipTypeBenefitId benfitId)
    {
        var benefit = _benefits.FirstOrDefault(b => b.Id == benfitId) ??
            throw new BenefitNotFoundException(benfitId);

        _benefits.Remove(benefit);

        UpdateTimeStamp();
    }
}