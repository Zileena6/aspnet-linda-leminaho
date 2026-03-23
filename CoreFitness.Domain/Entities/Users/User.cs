using CoreFitness.Domain.Entities.Base;

namespace CoreFitness.Domain.Entities.Users;

public class User : BaseEntity<UserId>
{
    // register account
    // LogIn / LogOut
    // System should handle auth with ASP.NET Core Identity

    protected User(UserId id, string firstName, string lastName, string email, string? phoneNumber, string password, string confirmPassword)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Password = password;
        ConfirmPassword = confirmPassword;
        
    }

    protected User() { }

    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? PhoneNumber { get; private set; }
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}
