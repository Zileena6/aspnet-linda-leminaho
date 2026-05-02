namespace CoreFitness.Application.DTOs.Auth;

public record LoginDTO
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public bool RememberMe { get; init; }
    public string? ReturnUrl { get; init; }
}