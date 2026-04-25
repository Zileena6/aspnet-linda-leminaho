using CoreFitness.Domain.Entities.Memberships.ValueObjects;

namespace CoreFitness.Domain.Exceptions;

public sealed class MembershipNotFoundException : NotFoundException
{
    public MembershipNotFoundException(MembershipId id) : base($"Membership with {id} was not found") { }
}

public sealed class InvalidExtendMembershipException : ValidationException
{
    public InvalidExtendMembershipException(string reason) : base($"Invalid membership extension: {reason}") { }
}

public sealed class NoSessionsLeftException : DomainException
{
    public NoSessionsLeftException() : base("No sessions left on membership") { }
}

public sealed class MembershipExpiredException : BusinessRuleException
{
    public MembershipExpiredException() : base("Membership has expired") { }
}

public sealed class MembershipAlreadyDeactivatedException : BusinessRuleException
{
    public MembershipAlreadyDeactivatedException() : base("Membership is already deactivated") { }
}