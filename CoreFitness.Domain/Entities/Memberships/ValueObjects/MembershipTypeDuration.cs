using CoreFitness.Domain.Exceptions;

namespace CoreFitness.Domain.Entities.Memberships.ValueObjects;

public readonly record struct MembershipTypeDuration
{
    public int Value { get; }

    private MembershipTypeDuration(int value)
    {
        Value = value;
    }

    public static MembershipTypeDuration Create(int value)
    {
        if (value < 0)
            throw new InvalidMembershipTypeDurationException(value);

        return new MembershipTypeDuration(value);
    }

    public static implicit operator int(MembershipTypeDuration duration) => duration.Value;
}
