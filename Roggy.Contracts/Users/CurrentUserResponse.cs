namespace Roggy.Contracts.Users;

public sealed record CurrentUserResponse(
    Guid Id,
    string Username,
    string Email,
    string DisplayName,
    string? ProfileImageUrl,
    string? Bio,
    string Role,
    bool IsActive);