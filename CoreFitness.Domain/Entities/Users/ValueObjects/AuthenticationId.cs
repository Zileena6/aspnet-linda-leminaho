using CoreFitness.Domain.Exceptions;

namespace CoreFitness.Domain.Entities.Users.ValueObjects;

public readonly record struct AuthenticationId
{
    public string Value { get; }

    public AuthenticationId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new IdIsRequiredException();

        Value = value;
    }

    public static AuthenticationId Create(string value) => new(value);

    public override string ToString() => Value.ToString();
}
