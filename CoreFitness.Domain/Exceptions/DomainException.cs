namespace CoreFitness.Domain.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException(string message, Exception? innerException = null) : base(message, innerException) { }
}

public abstract class NotFoundException : DomainException
{
    protected NotFoundException(string message) : base(message) { }
}

public abstract class ValidationException : DomainException
{
    protected ValidationException(string message) : base(message) { }
}

public abstract class BusinessRuleException : DomainException
{
    protected BusinessRuleException(string message) : base(message) { }
}

public abstract class ConflictException : DomainException
{
    protected ConflictException(string message) : base(message) { }
}