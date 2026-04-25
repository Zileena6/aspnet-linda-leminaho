namespace CoreFitness.Domain.Exceptions;

public sealed class IdIsRequiredException : ValidationException
{
    public IdIsRequiredException() : base("Id is required") { }
}

public class InvalidNameException : ValidationException
{
    public InvalidNameException(string name) : base($"Name {name} is invalid") { }
}
