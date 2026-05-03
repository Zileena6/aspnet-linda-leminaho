using CoreFitness.Application.DTOs.FaqItem;
using CoreFitness.Application.DTOs.Membership;

namespace CoreFitness.Web.ViewModels.Membership;

public class MembershipPageViewModel
{
    public IEnumerable<MembershipTypeDTO> MembershipTypes { get; init; } = [];
    public IEnumerable<FaqItemDTO> FaqItems { get; init; } = [];
}
