using CoreFitness.Domain.Exceptions;

namespace CoreFitness.Domain.Entities.TrainingSessions.ValueObjects;

public readonly record struct TrainingSessionDescription
{
    public const int MaxLength = 500;
    public string Value { get; }

    private TrainingSessionDescription(string value)
    {
        Value = value;
    }

    public static TrainingSessionDescription Create(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new InvalidTrainingSessionDescriptionException(description);

        var trimmed = description.Trim();

        if (trimmed.Length > MaxLength)
            throw new InvalidDescriptionLengthException();

        return new TrainingSessionDescription(trimmed);
    }

    public override string ToString() => Value;
}