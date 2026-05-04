using System.ComponentModel.DataAnnotations;

namespace CoreFitness.Web.ViewModels.Admin.Sessions;

public class CreateSessionViewModel
{
    [Required(ErrorMessage = "Session name is required")]
    [StringLength(100, ErrorMessage = "Name can not exceed 100 characters")]
    public string Name { get; init; } = string.Empty;

    [Required(ErrorMessage = "Session name is required")]
    [StringLength(500, ErrorMessage = "Description can not exceed 500 characters")]
    public string Description { get; init; } = string.Empty;

    [Range(1, 50, ErrorMessage = "Capacity must be between 1 and 50")]
    public int Capacity { get; init; }

    [Required(ErrorMessage = "Start date is required")]
    public DateTime StartDate { get; init; }

    [Range(30, 90, ErrorMessage = "Duration must be between 30 and 90 minutes")]
    public int DurationInMinutes { get; init; }
}