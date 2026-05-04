using CoreFitness.Application.DTOs.TrainingSession;

namespace CoreFitness.Web.ViewModels.Admin.Sessions;

public class AdminSessionsViewModel
{
    public IEnumerable<TrainingSessionDTO> Sessions { get; set; } = [];
}