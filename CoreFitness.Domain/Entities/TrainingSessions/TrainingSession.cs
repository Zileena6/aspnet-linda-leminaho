using CoreFitness.Domain.Common;
using CoreFitness.Domain.Entities.Bookings;
using CoreFitness.Domain.Entities.Common;
using CoreFitness.Domain.Entities.TrainingSessions.ValueObjects;
using CoreFitness.Domain.Entities.Users.ValueObjects;
using CoreFitness.Domain.Exceptions;
using CoreFitness.Domain.Interfaces;

namespace CoreFitness.Domain.Entities.TrainingSessions;

public class TrainingSession : BaseEntity<TrainingSessionId>, IAggregateRoot
{
    public TrainingSessionName TrainingSessionName { get; private set; }
    public TrainingSessionDescription TrainingSessionDescription { get; private set; }
    public TrainingSessionCapacity Capacity { get; private set; } = null!;

    public DateTimeOffset StartDate { get; private set; }
    public TrainingSessionDuration Duration { get; private set; }
    public DateTimeOffset EndDate => StartDate.Add(Duration.Value);

    private readonly List<Booking> _bookings = new();
    public virtual IReadOnlyCollection<Booking> Bookings => _bookings.AsReadOnly();
    public bool IsFull => _bookings.Count >= Capacity.Value;

    private TrainingSession() { }

    private TrainingSession(
        TrainingSessionId id, 
        TrainingSessionName name, 
        TrainingSessionDescription description, 
        DateTimeOffset startDate, 
        TrainingSessionCapacity capacity, 
        TrainingSessionDuration duration)
    {
        Id = id;
        TrainingSessionName = name;
        TrainingSessionDescription = description;
        StartDate = startDate;
        Capacity = capacity;
        Duration = duration;
    }

    public static TrainingSession Create(
        TrainingSessionName name, 
        TrainingSessionDescription description, 
        DateTimeOffset startDate, 
        TrainingSessionCapacity capacity, 
        TrainingSessionDuration duration)
    {
        if (startDate <= DateTimeOffset.UtcNow) throw new InvalidStartDateException();

        return new TrainingSession(
            TrainingSessionId.New(),
            name,
            description,
            startDate,
            capacity,
            duration);
    }

    public Result<Booking> Book(UserId userId)
    {
        if (IsFull)
            return Result<Booking>.Conflict("This session is full");

        if (_bookings.Any(b => b.UserId == userId))
            return Result<Booking>.Conflict("User is already booked");

        var booking = Booking.Create(userId, Id);

        _bookings.Add(booking);

        UpdateTimeStamp();

        return Result<Booking>.Success(booking);
    }

    public Result CancelBooking(UserId userId)
    {
        var booking = _bookings.FirstOrDefault(b => b.UserId == userId);

        if (booking is null)
            return Result.NotFound("Booking", userId);

        _bookings.Remove(booking);

        UpdateTimeStamp();

        return Result.Success();
    }

    public void UpdateName(TrainingSessionName newTrainingSessionName)
    {
        if (TrainingSessionName == newTrainingSessionName) return;

        TrainingSessionName = newTrainingSessionName;

        UpdateTimeStamp();
    }

    public void UpdateDescription(TrainingSessionDescription newDesctription)
    {
        if (TrainingSessionDescription == newDesctription) return;

        TrainingSessionDescription = newDesctription;

        UpdateTimeStamp();
    }

    public void UpdateStartDate(DateTimeOffset startDate)
    {
        if (startDate <= DateTimeOffset.UtcNow)
            throw new InvalidStartDateException();

        StartDate = startDate;

        UpdateTimeStamp();
    }

    public void UpdateDuration(TrainingSessionDuration newDuration)
    {
        if (Duration == newDuration) return;

        Duration = newDuration;

        UpdateTimeStamp();
    }

    public void UpdateCapacity(TrainingSessionCapacity newCapacity)
    {
        if (newCapacity.Value < _bookings.Count)
            throw new InvalidCapacityException(newCapacity.Value);

        if (Capacity == newCapacity) return;

        Capacity = newCapacity;

        UpdateTimeStamp();
    }

    public void UpdateAll(
        TrainingSessionName name, 
        TrainingSessionDescription description, 
        DateTimeOffset start, 
        TrainingSessionDuration duration, 
        TrainingSessionCapacity capacity)
    {
        UpdateName(name);
        UpdateDescription(description);
        UpdateStartDate(start);
        UpdateDuration(duration);
        UpdateCapacity(capacity);
    }
}