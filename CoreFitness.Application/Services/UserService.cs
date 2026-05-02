using CoreFitness.Application.Authentication.Abstractions;
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

public class UserService(
    IUserRepository repository, 
    IUnitOfWork unitOfWork, 
    IFileStorage fileStorage, 
    IPasswordProvider passwordProvider) : IUserService
{
    public async Task<Result> DeleteAsync(Guid userId, CancellationToken ct = default)
    {
        var deleted = await repository.DeleteAsync(new UserId(userId), ct);

        if (!deleted)
            return Result.Failure(Error.NotFound("User", userId));

        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> DeleteAccountAsync(AuthenticationId authId, CancellationToken ct = default)
    {
        var user = await repository.GetByAuthenticationIdAsync(authId, ct);

        if (user is null)
            return Result.Failure(Error.NotFound("User", authId));

        var identityResult = await passwordProvider.DeleteUserAsync(authId.Value, ct);

        if (!identityResult.IsSuccess)
            return identityResult;

        await repository.DeleteAsync(user.Id, ct);
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

    public async Task<Result> UpdateProfileAsync(UpdateProfileDTO dto, CancellationToken ct = default)
    {
        var user = await repository.GetByIdAsync(new UserId(dto.Id), ct);

        if (user is null)
            return Result.Failure(Error.NotFound("User", dto.Id));

        if (dto.Email is not null && dto.Email != user.Email.Value)
        {
            var email = UserEmail.Create(dto.Email);

            if (await repository.ExistsByEmailAsync(email, ct))
                return Result.Failure(Error.Conflict($"Email {dto.Email} is already in use"));

            user.UpdateEmail(email);
        }

        if (dto.FirstName is not null)
            user.UpdateFirstName(dto.FirstName);

        if (dto.LastName is not null)
            user.UpdateLastName(dto.LastName);

        if (dto.PhoneNumber is not null)
            user.UpdatePhoneNumber(UserPhoneNumber.Create(dto.PhoneNumber));

        if (dto.Weight.HasValue && dto.Height.HasValue)
            user.UpdateWeight(dto.Weight.Value, dto.Height.Value);

        if (dto.TargetWeight.HasValue)
            user.SetWeightGoal(dto.TargetWeight.Value);

        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> CompleteRegistrationAsync(AuthenticationId authId, CompleteProfileDTO dto, CancellationToken ct = default)
    {
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

    public async Task<Result<UserStatisticsDTO>> GetStatisticsAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await repository.GetByIdAsync(new UserId(userId), ct);

        if (user is null)
            return Result<UserStatisticsDTO>.Failure(Error.NotFound("User", userId));

        return Result<UserStatisticsDTO>.Success(new UserStatisticsDTO
        {
            CurrentWeight = user.CurrentWeight,
            TargetWeight = user.TargetWeight,
            Height = user.Height,
            BMI = user.BMI
        });
    }

    public async Task<Result<UserDTO>> GetByAuthenticationId(AuthenticationId authId, CancellationToken ct = default)
    {
        var user = await repository.GetByAuthenticationIdAsync(authId, ct);

        if (user is null)
            return Result<UserDTO>.Failure(Error.NotFound("User", authId));

        return Result<UserDTO>.Success(user.ToDTO());
    }

    public async Task<Result> UpdateWeightAsync(
        Guid userId, 
        decimal weight, 
        decimal height, 
        CancellationToken ct = default)
    {
        var user = await repository.GetByIdAsync(new UserId(userId), ct);

        if (user is null)
            return Result.Failure(Error.NotFound("User", userId));

        user.UpdateWeight(weight, height);

        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> UpdateWeightGoalAsync(Guid userId, decimal targetWeight, CancellationToken ct = default)
    {
        var user = await repository.GetByIdAsync(new UserId(userId), ct);

        if (user is null)
            return Result.Failure(Error.NotFound("User", userId));

        user.SetWeightGoal(targetWeight);

        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> UploadProfilePhotoAsync(
        AuthenticationId authId, 
        Stream stream, 
        string originalFileName, 
        CancellationToken ct = default)
    {
        var user = await repository.GetByAuthenticationIdAsync(authId, ct);

        if (user is null)
            return Result.Failure(Error.NotFound("User", authId));

        var extension = Path.GetExtension(originalFileName)?.ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(extension) || extension is not (".jpg" or ".jpeg" or ".png" or ".webp"))
            return Result.Failure(Error.Validation("Invalid file type"));

        var fileName = $"{Guid.NewGuid()}{extension}";

        var photoUrl = await fileStorage.SaveAsync(stream, fileName, ct);

        user.UpdatePhotoUrl(photoUrl);

        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}