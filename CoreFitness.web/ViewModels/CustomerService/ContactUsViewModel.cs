using System.ComponentModel.DataAnnotations;
using CoreFitness.Application.DTOs.FaqItem;

namespace CoreFitness.Web.ViewModels.CustomerService;

public class ContactUsViewModel
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name can not exceed 50 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name can not exceed 50 characters")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Message is required")]
    [StringLength(1000, MinimumLength = 10, ErrorMessage = "Message must be between 10 and 1000 characters")]
    public string Message { get; set; } = string.Empty;

    public IEnumerable<FaqItemDTO> FaqItems { get; init; } = [];
}
