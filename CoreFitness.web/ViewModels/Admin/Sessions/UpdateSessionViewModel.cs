using System.ComponentModel.DataAnnotations;

namespace CoreFitness.Web.ViewModels.Admin.Sessions;

public class UpdateSessionViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name can not exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    [StringLength(500, ErrorMessage = "Description can not exceed 500 characters")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Start date is required")]
    public DateTime StartDate { get; set; }

    [Range(1, 50, ErrorMessage = "Capacity must be between 1 and 50")]
    public int Capacity { get; set; }
    
    [Range(30, 90, ErrorMessage = "Duration must be between 30 and 90 minutes")]
    public int DurationInMinutes { get; set; }
}