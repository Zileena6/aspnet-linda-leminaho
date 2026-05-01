using CoreFitness.Domain.Exceptions;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CoreFitness.Domain.Entities.Users.ValueObjects;

public readonly partial record struct UserEmail
{
    public string Value { get; }
    public string UniqueValue { get; }

    private UserEmail(string value, string uniqueValue)
    {
        Value = value;
        UniqueValue = uniqueValue;
    }

    [GeneratedRegex(
        @"^(?=.{3,254}$)(?=.{1,64}@)" +
        @"[\p{L}\p{N}]+([._%+\-][\p{L}\p{N}]+)*" +
        @"@" +
        @"(?:[A-Z0-9](?:[A-Z0-9\-]{0,61}[A-Z0-9])?\.)+" +
        @"(?:[A-Z]{2,63}|XN--[A-Z0-9\-]{2,59})$",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]

    private static partial Regex EmailRegex();

    public static UserEmail Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidUserEmailException("Email is required");

        var trimmed = value.Trim();

        string normalized = NormalizeForDomainValidation(trimmed);

        if (!EmailRegex().IsMatch(normalized))
            throw new InvalidUserEmailException("Invalid email format");

        var unique = CreateUniqueKey(normalized);

        return new UserEmail(trimmed.ToLowerInvariant(), unique);
    }

    private static string NormalizeForDomainValidation(string userEmail)
    {
        var at = userEmail.IndexOf('@');

        if (at <= 0 || at != userEmail.LastIndexOf('@') || at == userEmail.Length - 1)
            return userEmail;

        var local = userEmail[..at];
        var domain = userEmail[(at + 1)..];

        try
        {
            var idn = new IdnMapping();
            domain = idn.GetAscii(domain);
        }
        catch (ArgumentException) { return userEmail; }

        return $"{local}@{domain}";
    }

    private static string CreateUniqueKey(string normalizedUserEmail)
    {
        var at = normalizedUserEmail.IndexOf('@');

        if (at <= 0 || at != normalizedUserEmail.LastIndexOf('@') || at == normalizedUserEmail.Length - 1)
            return normalizedUserEmail.ToLowerInvariant();

        var local = normalizedUserEmail[..at].ToLowerInvariant();
        var domain = normalizedUserEmail[(at + 1)..];

        var plus = local.LastIndexOf('+');

        if (plus >= 0) local = local[..plus];

        if (domain is "gmail.com" or "googlemail.com")
            local = local.Replace(".", "");

        return $"{local}@{domain}";
    }

    public bool Equals(UserEmail other) => UniqueValue == other.UniqueValue;

    public override int GetHashCode() => UniqueValue.GetHashCode();

    public override string ToString() => Value;
}
