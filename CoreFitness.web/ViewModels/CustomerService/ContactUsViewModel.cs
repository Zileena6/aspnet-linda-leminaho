using System.ComponentModel.DataAnnotations;

namespace CoreFitness.web.ViewModels.CustomerService;

public class ContactUsViewModel
{
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Message is required")]
    public string Message { get; set; } = string.Empty;
}
