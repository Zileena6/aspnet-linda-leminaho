using CoreFitness.Domain.Exceptions;
using CoreFitness.Domain.Primitives;
using System.Text.RegularExpressions;

namespace CoreFitness.Domain.Entities.Users.ValueObjects;

public readonly partial record struct UserName
{
    public const int MaxLength = 50;
    public string FirstName { get; }
    public string LastName { get; }

    private UserName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static UserName Create(string firstName, string lastName)
    {
        var cleanFirstName = firstName.NormalizeName();
        var cleanLastName = lastName.NormalizeName();

        if (cleanFirstName.Length > MaxLength || cleanLastName.Length > MaxLength)
            throw new InvalidNameException($"Name can not exceed {MaxLength} characters");

        if (!string.IsNullOrEmpty(cleanFirstName) && !NameRegex().IsMatch(cleanFirstName) ||
            !string.IsNullOrEmpty(cleanLastName) && !NameRegex().IsMatch(cleanLastName))
            throw new InvalidNameException("Name contains invalid characters");

        return new UserName(cleanFirstName, cleanLastName);
    }

    [GeneratedRegex(@"^[\p{L}]+(?:[ '-][\p{L}]+)*$")]
    private static partial Regex NameRegex();

    public string Fullname => $"{FirstName} {LastName}";

    public override string ToString() => Fullname;
}