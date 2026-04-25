using CoreFitness.Domain.Exceptions;

namespace CoreFitness.Domain.Entities.Users.ValueObjects;

public readonly record struct UserId
{
    public Guid Value { get; }

    public UserId(Guid value)
    {
        if (value == Guid.Empty)
            throw new IdIsRequiredException();

        Value = value;
    }

    public static UserId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
