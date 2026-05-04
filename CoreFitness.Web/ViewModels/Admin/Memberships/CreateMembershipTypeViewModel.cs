namespace CoreFitness.Web.ViewModels.Admin.Memberships;

public class CreateMembershipTypeViewModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int DurationInDays { get; set; }
    public int SessionLimit { get; set; }
}