namespace CoreFitness.Web.ViewModels.Admin.Memberships;

public class MembershipTypeFormViewModel
{
    public Guid? Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public int DurationInDays { get; init; }
    public int SessionLimit { get; init; }

    public bool IsEdit => Id.HasValue;
}