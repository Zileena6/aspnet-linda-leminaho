using CoreFitness.Application.DTOs.User;

namespace CoreFitness.web.ViewModels.Profile;

public class ProfilePageViewModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? PhotoUrl { get; set; }
    public decimal? Weight { get; set; }
    public decimal? Height { get; set; }
    public decimal? TargetWeight { get; set; }
    public UserStatisticsDTO? Statistics { get; set; }
}
