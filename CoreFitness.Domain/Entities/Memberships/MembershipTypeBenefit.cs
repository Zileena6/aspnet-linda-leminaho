using CoreFitness.Domain.Entities.Common;
using CoreFitness.Domain.Entities.Memberships.ValueObjects;

namespace CoreFitness.Domain.Entities.Memberships;

public class MembershipTypeBenefit : BaseEntity<MembershipTypeBenefitId>
{
    public MembershipTypeId MembershipTypeId { get; private set; }
    public MembershipTypeBenefitDescription Description { get; private set; }

    private MembershipTypeBenefit() { }

    private MembershipTypeBenefit(MembershipTypeBenefitId id, MembershipTypeId membershipTypeId, MembershipTypeBenefitDescription description)
    {
        Id = id;
        MembershipTypeId = membershipTypeId;
        Description = description;
    }

    public static MembershipTypeBenefit Create(MembershipTypeId membershipTypeId, MembershipTypeBenefitDescription description) =>
        new(MembershipTypeBenefitId.New(), membershipTypeId, description);
}