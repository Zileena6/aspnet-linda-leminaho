namespace CoreFitness.Web.ViewModels.Admin.Sessions;

public class UpdateSessionViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public int Capacity { get; set; }
    public int DurationInMinutes { get; set; }
}