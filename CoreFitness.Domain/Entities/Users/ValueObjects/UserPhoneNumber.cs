using CoreFitness.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace CoreFitness.Domain.Entities.Users.ValueObjects;

public readonly partial record struct UserPhoneNumber
{
    public string Value { get; }
    private UserPhoneNumber(string value) => Value = value;

    public static UserPhoneNumber? Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;

        var trimmed = MyRegex().Replace(value.Trim(), "");

        if (!UserPhoneNumberRegex().IsMatch(trimmed))
            throw new InvalidUserPhoneNumberException("Invalid phone number format, e.g. +46123321456");

        return new UserPhoneNumber(trimmed);
    }

    [GeneratedRegex(@"^\+[1-9]\d{7,14}$")]
    private static partial Regex UserPhoneNumberRegex();

    [GeneratedRegex(@"[\s\-\.\(\)]")]
    private static partial Regex MyRegex();

    public override string ToString() => Value;
}
