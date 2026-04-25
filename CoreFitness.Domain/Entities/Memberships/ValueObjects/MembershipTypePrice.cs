using CoreFitness.Domain.Exceptions;

namespace CoreFitness.Domain.Entities.Memberships.ValueObjects;

public readonly record struct MembershipTypePrice
{
    public decimal Value { get; }

    private MembershipTypePrice(decimal value)
    {
        Value = value;
    }

    public static MembershipTypePrice Create(decimal value)
    {
        if (value < 0)
            throw new InvalidPriceException(value);

        return new MembershipTypePrice(value);
    }

    public static implicit operator decimal(MembershipTypePrice price) => price.Value;
}