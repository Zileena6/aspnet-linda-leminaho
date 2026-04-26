namespace CoreFitness.Application.DTOs.TrainingSession;

public record TrainingSessionDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int Capacity { get; init; }
    public DateTimeOffset StartDate { get; init; }
    public int DurationInMinutes { get; init; }
    public bool IsFull { get; init; }
    public int CurrentBookings { get; init; }

    public DateTimeOffset EndDate => StartDate.AddMinutes(DurationInMinutes);
    public int AvailableSpots => Capacity - CurrentBookings;
}
