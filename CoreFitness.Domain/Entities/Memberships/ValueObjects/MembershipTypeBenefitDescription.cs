using CoreFitness.Domain.Exceptions;
using CoreFitness.Domain.Primitives;

namespace CoreFitness.Domain.Entities.Memberships.ValueObjects;

public readonly record struct MembershipTypeBenefitDescription
{
    public string Value { get; }

    public const int MaxLength = 500;

    private MembershipTypeBenefitDescription(string value)
    {
        Value = value;
    }

    public static MembershipTypeBenefitDescription Create(string value)
    {
        var cleanDescription = value.NormalizeText();

        if (cleanDescription.Length > MaxLength)
            throw new InvalidBenefitDescriptionException();

        return new MembershipTypeBenefitDescription(cleanDescription);
    }

    public override string ToString() => Value;
}
