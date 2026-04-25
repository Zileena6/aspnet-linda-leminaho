using CoreFitness.Domain.Exceptions;

namespace CoreFitness.Domain.Entities.Memberships.ValueObjects;

public readonly record struct MembershipTypeBenefitId
{
    public Guid Value { get; }

    public MembershipTypeBenefitId(Guid value)
    {
        if (value == Guid.Empty)
            throw new IdIsRequiredException();

        Value = value;
    }

    public static MembershipTypeBenefitId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
