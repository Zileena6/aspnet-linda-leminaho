using CoreFitness.Application.DTOs.Membership;

namespace CoreFitness.Web.ViewModels.Admin.Memberships;

public class AdminMembershipsViewModel
{
    public IEnumerable<MembershipTypeDTO> MembershipTypes { get; set; } = [];
    public MembershipTypeFormViewModel? UpdateMembershipType { get; set; }
}