using CoreFitness.Domain.Exceptions;

namespace CoreFitness.Domain.Entities.Memberships.ValueObjects;

public readonly record struct CheckInId
{
    public Guid Value { get; }

    public CheckInId(Guid value)
    {
        if (value == Guid.Empty)
            throw new IdIsRequiredException();

        Value = value;
    }

    public static CheckInId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();
}
