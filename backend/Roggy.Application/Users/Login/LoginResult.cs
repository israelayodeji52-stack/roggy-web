namespace Roggy.Application.Users.Login;

public sealed record LoginResult(
    Guid UserId,
    string Username,
    string Email,
    string DisplayName,
    string Token);