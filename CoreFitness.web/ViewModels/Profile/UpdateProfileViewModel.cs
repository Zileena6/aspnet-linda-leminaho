namespace CoreFitness.Web.ViewModels.Profile;

public class UpdateProfileViewModel
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public decimal? Weight { get; set; }
    public decimal? Height { get; set; }
    public decimal? TargetWeight { get; set; }
    public byte[] RowVersion { get; set; } = default!;
}
