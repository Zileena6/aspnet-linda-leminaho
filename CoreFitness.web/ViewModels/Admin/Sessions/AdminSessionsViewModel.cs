using CoreFitness.Application.DTOs.TrainingSession;

namespace CoreFitness.web.ViewModels.Admin.Sessions;

public class AdminSessionsViewModel
{
    public IEnumerable<TrainingSessionDTO> Sessions { get; set; } = [];
}