using CoreFitness.Application.DTOs.Booking;
using CoreFitness.Application.DTOs.TrainingSession;
using CoreFitness.Domain.Entities.Bookings;
using CoreFitness.Domain.Entities.TrainingSessions;

namespace CoreFitness.Application.Mappings;

public static class TrainingSessionMappings
{
    public static TrainingSessionDTO ToDTO(this TrainingSession session) => new()
    {
        Id = session.Id.Value,
        Name = session.TrainingSessionName.Value,
        Description = session.TrainingSessionDescription.Value,
        Capacity = session.Capacity.Value,
        StartDate = session.StartDate,
        DurationInMinutes = session.Duration.TotalMinutes,
        CurrentBookings = session.Bookings.Count,
        IsFull = session.IsFull
    };

    public static BookingDTO ToDTO(this Booking booking, TrainingSession session) => new()
    {
        Id = booking.Id.Value,
        TrainingSessionId = session.Id.Value,
        SessionName = session.TrainingSessionName.Value,
        StartDate = session.StartDate,
        DurationInMinutes = session.Duration.TotalMinutes
    };
}
