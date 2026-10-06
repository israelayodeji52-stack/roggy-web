namespace Roggy.Contracts.Users;

public sealed record UserResponse(
    Guid Id,
    string Username,
    string Email,
    string DisplayName);