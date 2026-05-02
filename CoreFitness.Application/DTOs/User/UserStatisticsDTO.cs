namespace CoreFitness.Application.DTOs.User;

public record UserStatisticsDTO
{
    public decimal? CurrentWeight { get; init; }
    public decimal? TargetWeight { get; init; }
    public decimal? Height { get; init; }
    public decimal? BMI { get; init; }
}