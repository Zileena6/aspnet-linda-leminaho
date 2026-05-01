using CoreFitness.Domain.Exceptions;

namespace CoreFitness.Domain.Entities.Memberships.ValueObjects;

public readonly record struct MembershipTypeId
{
    public Guid Value { get; }

    public MembershipTypeId(Guid value)
    {
        if (value == Guid.Empty)
            throw new IdIsRequiredException();

        Value = value;
    }

    public static MembershipTypeId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();
}
