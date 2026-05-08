using CoreFitness.Application.DTOs.Membership;
using CoreFitness.Application.DTOs.User;
using CoreFitness.Application.DTOs.Booking;

namespace CoreFitness.Web.ViewModels.Profile;

public class ProfilePageViewModel
{
    public Guid Id { get; set; }


    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }

    public string? PhotoUrl { get; set; }

    public decimal? Weight { get; set; }
    public decimal? Height { get; set; }
    public decimal? TargetWeight { get; set; }

    public UserStatisticsDTO? Statistics { get; set; }
    public MembershipDTO? Membership { get; set; }
    public ProfileTabs ActiveTab { get; set; }
    public IEnumerable<BookingDTO> Bookings { get; set; } = [];

    public UpdateProfileViewModel UpdateForm { get; set; } = new();
}
