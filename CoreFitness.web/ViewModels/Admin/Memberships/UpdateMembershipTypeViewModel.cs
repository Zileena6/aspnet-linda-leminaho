using System.ComponentModel.DataAnnotations;

namespace CoreFitness.Web.ViewModels.Admin.Memberships;

public class UpdateMembershipTypeViewModel
{
    public Guid Id { get; init; }
    
    [Required(ErrorMessage = "Session name is required")]
    [StringLength(100, ErrorMessage = "Name can not exceed 100 characters")]
    public string Name { get; init; } = string.Empty;

    [Required(ErrorMessage = "Session name is required")]
    [StringLength(500, ErrorMessage = "Description can not exceed 500 characters")]
    public string Description { get; init; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greate than 0" )]
    public decimal Price { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "Duration must be greater than 0")]
    public int DurationInDays { get; init; }
    
    [Range(0, int.MaxValue, ErrorMessage = "Session limit can not be a negative value")]
    public int SessionLimit { get; init; }
}