namespace CoreFitness.Application.DTOs.Booking;

public record BookingDTO
{
    public Guid Id { get; init; }
    public Guid TrainingSessionId { get; init; }
    public string SessionName { get; init; } = string.Empty;
    public DateTimeOffset StartDate { get; init; }
    public int DurationInMinutes { get; init; }
    public DateTimeOffset EndDate => StartDate.AddMinutes(DurationInMinutes);
}