namespace CoreFitness.Domain.Exceptions;

public class MissingRowVersionException : DomainException
{
    public MissingRowVersionException() : base("RowVersion is required") { }
}