using CoreFitness.Application.DTOs.Membership;

namespace CoreFitness.web.ViewModels.Admin.Memberships;

public class AdminMembershipsViewModel
{
    public IEnumerable<MembershipTypeDTO> MembershipTypes { get; set; } = [];
    public MembershipTypeFormViewModel? UpdateMembershipType { get; set; }
}