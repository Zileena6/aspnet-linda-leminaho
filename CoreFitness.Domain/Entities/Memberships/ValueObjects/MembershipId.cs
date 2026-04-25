using CoreFitness.Domain.Exceptions;

namespace CoreFitness.Domain.Entities.Memberships.ValueObjects;

public readonly record struct MembershipId
{
    public Guid Value { get; }

    public MembershipId(Guid value)
    {
        if (value == Guid.Empty)
            throw new IdIsRequiredException();

        Value = value;
    }

    public static MembershipId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
