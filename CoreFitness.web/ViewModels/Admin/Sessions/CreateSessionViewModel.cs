namespace CoreFitness.Web.ViewModels.Admin.Sessions;

public class CreateSessionViewModel
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int Capacity { get; init; }
    public DateTime StartDate { get; init; }
    public int DurationInMinutes { get; init; }
}