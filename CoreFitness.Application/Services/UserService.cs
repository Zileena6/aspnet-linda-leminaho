using CoreFitness.Application.DTOs.User;
using CoreFitness.Application.Interfaces;
using CoreFitness.Application.Mappings;
using CoreFitness.Domain.Common;
using CoreFitness.Domain.Entities.Users;
using CoreFitness.Domain.Entities.Users.ValueObjects;
using CoreFitness.Domain.Enums;
using CoreFitness.Domain.Interfaces.UnitOfWork;
using CoreFitness.Domain.Interfaces.Users;

namespace CoreFitness.Application.Services;

public class UserService(IUserRepository repository, IUnitOfWork unitOfWork) : IUserService
{
    public async Task<Result> DeleteAsync(Guid userId, CancellationToken ct = default)
    {
        var deleted = await repository.DeleteAsync(new UserId(userId), ct);

        if (!deleted)
            return Result.Failure(Error.NotFound("User", userId));

        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }

    public async Task<Result<UserDTO>> GetByIdAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await repository.GetByIdAsync(new UserId(userId), ct);

        if (user is null)
            return Result<UserDTO>.Failure(Error.NotFound("User", userId));

        return Result<UserDTO>.Success(user.ToDTO());
    }

    public async Task<Result> UpdateAsync(UpdateUserDTO dto, CancellationToken ct = default)
    {
        var user = await repository.GetByIdAsync(new UserId(dto.Id), ct);

        if (user is null)
            return Result.Failure(Error.NotFound("User", dto.Id));

        if (dto.Email is not null && dto.Email != user.Email.Value)
        {
            var newEmail = UserEmail.Create(dto.Email);

            if (await repository.ExistsByEmailAsync(newEmail, ct))
                return Result.Failure(Error.Conflict($"Email {dto.Email} is already in use"));

            user.UpdateEmail(newEmail);
        }

        user.UpdateFirstName(dto.FirstName ?? user.UserName.FirstName);
        user.UpdateLastName(dto.LastName ?? user.UserName.LastName);

        if (dto.PhoneNumber is not null && dto.PhoneNumber != user.UserPhoneNumber?.Value)
        {
            user.UpdatePhoneNumber(UserPhoneNumber.Create(dto.PhoneNumber));
        }

        if (dto.PhotoUrl is not null)
            user.UpdatePhotoUrl(dto.PhotoUrl);

        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }

    public async Task<Result> CompleteRegistrationAsync(Guid authenticationId, CompleteProfileDTO dto, CancellationToken ct = default)
    {
        var authId = AuthenticationId.Create(authenticationId.ToString());

        var user = User.Create(
            authId,
            UserEmail.Create(dto.Email),
            UserName.Create(dto.FirstName, dto.LastName),
            UserPhoneNumber.Create(dto.PhoneNumber),
            dto.PhotoUrl,
            UserRole.Member);

        await repository.AddAsync(user, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }
}