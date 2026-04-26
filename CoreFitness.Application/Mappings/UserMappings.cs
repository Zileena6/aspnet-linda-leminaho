using CoreFitness.Application.DTOs.User;
using CoreFitness.Domain.Entities.Users;

namespace CoreFitness.Application.Mappings;

public static class UserMappings
{
    public static UserDTO ToDTO(this User user) => new()
    {
        Id = user.Id.Value,
        Email = user.Email.Value,
        FirstName = user.UserName.FirstName,
        LastName = user.UserName.LastName,
        PhoneNumber = user.UserPhoneNumber?.Value,
        PhotoUrl = user.PhotoUrl,
        Role = user.Role
    };
}
