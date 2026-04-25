using CoreFitness.Application.DTOs.Auth;
using CoreFitness.Domain.Common;

namespace CoreFitness.Application.Identity;

public interface IAuthService
{
    Task<Result> RegisterAsync(RegisterDTO dto, CancellationToken ct = default);
    Task<Result> LoginAsync(LoginDTO dto, CancellationToken ct = default);
    Task LogOutAsync();
}