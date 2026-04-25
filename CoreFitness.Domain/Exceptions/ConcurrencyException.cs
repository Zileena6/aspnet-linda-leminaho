namespace CoreFitness.Domain.Exceptions;

public sealed class ConcurrencyException(Exception? innerException = null) : DomainException("Someone just updated the session, please try again.", innerException);