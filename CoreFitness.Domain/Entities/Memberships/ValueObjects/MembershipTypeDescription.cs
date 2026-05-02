using CoreFitness.Domain.Exceptions;
using CoreFitness.Domain.Primitives;

namespace CoreFitness.Domain.Entities.Memberships.ValueObjects;

public readonly record struct MembershipTypeDescription
{
    public string Value { get; }
    public const int MaxLength = 1000;

    private MembershipTypeDescription(string value)
    {
        Value = value;
    }

    public static MembershipTypeDescription Create(string value)
    {
        var cleanDescription = value.NormalizeText();

        if (cleanDescription.Length > MaxLength)
            throw new InvalidMembershipTypeDescriptionException();

        return new MembershipTypeDescription(cleanDescription);
    }

    public override string ToString() => Value;
}