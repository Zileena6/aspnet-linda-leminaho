namespace CoreFitness.Application.DTOs.FaqItem;

public record FaqItemDTO
{
    public string Question { get; init; } = string.Empty;
    public string Answer { get; init; } = string.Empty;
    public List<string> Bullets { get; init; } = [];
}