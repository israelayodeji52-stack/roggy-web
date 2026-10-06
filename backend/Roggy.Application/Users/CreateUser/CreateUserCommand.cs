namespace Roggy.Application.Users.CreateUser;

public sealed record CreateUserCommand(
    string Username,
    string Email,
    string Password,
    string DisplayName);