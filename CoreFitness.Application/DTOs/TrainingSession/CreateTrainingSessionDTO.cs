namespace CoreFitness.Application.DTOs.TrainingSession;

public record CreateTrainingSessionDTO
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int Capacity { get; init; }
    public DateTimeOffset StartDate { get; init; }
    public int DurationInMinutes { get; init; }
}
