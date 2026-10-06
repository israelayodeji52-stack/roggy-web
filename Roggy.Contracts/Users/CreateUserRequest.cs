namespace Roggy.Contracts.Users;

public sealed record CreateUserRequest(
    string Username,
    string Email,
    string Password,
    string DisplayName);