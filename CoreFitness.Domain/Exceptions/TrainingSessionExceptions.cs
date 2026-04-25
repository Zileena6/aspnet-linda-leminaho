using CoreFitness.Domain.Entities.TrainingSessions.ValueObjects;
using CoreFitness.Domain.Entities.Users.ValueObjects;

namespace CoreFitness.Domain.Exceptions;

public sealed class TrainingSessionNotFoundException : NotFoundException
{
    public TrainingSessionNotFoundException(TrainingSessionId id) : base($"Training session with id {id} was not found") { }
}

public class TrainingSessionIsFullException : BusinessRuleException
{
    public TrainingSessionIsFullException() : base("No spots available in this training session") { }
}

public sealed class DuplicateBookingException : ConflictException
{
    public DuplicateBookingException() : base("User already booked for this session") { }
}

public sealed class InvalidStartDateException : ValidationException
{
    public InvalidStartDateException() : base("Start date is invalid") { }
}

public sealed class InvalidCapacityException : ValidationException
{
    public InvalidCapacityException(int capacity) : base($"Capacity {capacity} is invalid") { }
}

public sealed class InvalidDurationException : ValidationException
{
    public InvalidDurationException(TimeSpan duration) : base($"Duration {duration} is invalid") { }
}

public sealed class InvalidDescriptionLengthException : ValidationException
{
    public InvalidDescriptionLengthException() : base("Description is too long") { }
}

public sealed class BookingNotFoundException : NotFoundException
{
    public BookingNotFoundException(UserId id, TrainingSessionId sessionId) : base($"Booking for user {id} in session {sessionId} was not found") { }
}

public sealed class InvalidTrainingSessionNameException : ValidationException
{
    public InvalidTrainingSessionNameException(string name) : base($"Training session name {name} is invalid") { }
}

public sealed class InvalidTrainingSessionDescriptionException : ValidationException
{
    public InvalidTrainingSessionDescriptionException(string name) : base($"Training session name {name} is invalid") { }
}

public sealed class InvalidSessionLimitException : ValidationException
{
    public InvalidSessionLimitException(int limit) : base($"Session limit {limit} must be greater than 0") { }
}