using CoreFitness.Domain.Entities.Users.ValueObjects;

namespace CoreFitness.Domain.Exceptions;

public sealed class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(UserId id) : base($"User with id {id} was not found") { }
    public UserNotFoundException(UserEmail userEmail) : base($"User with email {userEmail.Value} was not found") { }
}

public sealed class EmailAlreadyExistsException : ConflictException
{
    public EmailAlreadyExistsException(UserEmail email) : base($"Email {email.Value} is already in use") { }
}

public sealed class InvalidUserEmailException : ValidationException
{
    public InvalidUserEmailException(string email) : base($"Email {email} is invalid") { }
}

public sealed class InvalidUserPhoneNumberException : ValidationException
{
    public InvalidUserPhoneNumberException(string phone) : base($"Phone number {phone} is invalid") { }
}

public sealed class InvalidPhotoUrlException : ValidationException
{
    public InvalidPhotoUrlException(string url) : base($"Photo URL {url} is invalid") { }
}

public class InvalidWeightException : ValidationException
{
    public InvalidWeightException(decimal weight) : base($"Weight {weight} is invalid") { }
}

public class InvalidHeightException : ValidationException
{
    public InvalidHeightException(decimal height) : base($"Height {height} is invalid") { }
}