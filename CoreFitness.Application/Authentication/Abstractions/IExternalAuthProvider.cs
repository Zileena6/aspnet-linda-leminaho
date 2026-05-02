using CoreFitness.Application.Authentication.Models;

namespace CoreFitness.Application.Authentication.Abstractions;

public interface IExternalAuthProvider
{
    Task<IReadOnlyList<string>> GetExternalProvidersAsync(CancellationToken ct = default);
    Task<ExternalUserInfo?> GetExternalUserAsync(CancellationToken ct = default);
    Task<ExternalSignInResult> SignInWithExternalAsync(string provider, string providerKey, CancellationToken ct = default);
    Task<LinkResult> LinkExternalLoginAsync(string provider, string providerKey, string userId, CancellationToken ct = default);
    Task<LinkResult> RemoveExternalLoginAsync(string provider, string providerKey, string userId, CancellationToken ct = default);
}