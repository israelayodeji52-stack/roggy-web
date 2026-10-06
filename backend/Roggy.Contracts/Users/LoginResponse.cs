namespace Roggy.Contracts.Users;

public sealed record LoginResponse(
    Guid UserId,
    string Username,
    string Email,
    string DisplayName,
    string Token);