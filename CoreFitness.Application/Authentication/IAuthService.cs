using CoreFitness.Application.Authentication.Models;
using CoreFitness.Application.DTOs.Auth;

namespace CoreFitness.Application.Authentication;

public interface IAuthService
{
    Task<AuthenticationResult> RegisterAsync(RegisterDTO dto, CancellationToken ct = default);
    Task<AuthenticationResult> LoginAsync(LoginDTO dto, CancellationToken ct = default);
    Task<IReadOnlyList<string>> GetExternalProvidersAsync(CancellationToken ct = default);
    Task<AuthenticationResult> HandleExternalCallbackAsync(string? returnUrl, string? remoteError, bool confirmed = false, CancellationToken ct = default);
    Task<AuthenticationResult> VerifyEmailAsync(string email, string code, string? returnUrl, CancellationToken ct = default);
    Task SignOutAsync(CancellationToken ct = default);
}