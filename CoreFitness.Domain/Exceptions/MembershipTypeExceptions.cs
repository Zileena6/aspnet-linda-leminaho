using CoreFitness.Domain.Entities.Memberships.ValueObjects;

namespace CoreFitness.Domain.Exceptions;

public sealed class MembershipTypeNotFoundException : NotFoundException
{
    public MembershipTypeNotFoundException(MembershipTypeId id) : base($"Membership type with id {id} was not found") { }
}
public sealed class InvalidMembershipTypeNameException : ValidationException
{
    public InvalidMembershipTypeNameException(string name) : base($"Membership type name {name} is invalid") { }
}
public sealed class InvalidMembershipTypeDescriptionException : ValidationException
{
    public InvalidMembershipTypeDescriptionException() : base("Membership type description is invalid") { }
}
public sealed class InvalidPriceException : ValidationException
{
    public InvalidPriceException(decimal price) : base($"Price {price} is invalid") { }
}
public sealed class InvalidMembershipTypeDurationException : ValidationException
{
    public InvalidMembershipTypeDurationException(int duration) : base($"Duration {duration} is invalid") { }
}
public class BenefitNotFoundException : NotFoundException
{
    public BenefitNotFoundException(MembershipTypeBenefitId id) : base($"Membership type benefit with id {id} was not found") { }
}
public sealed class InvalidBenefitDescriptionException : ValidationException
{
    public InvalidBenefitDescriptionException() : base("Benefit description is invalid") { }
}
