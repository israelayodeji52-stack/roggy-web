namespace Roggy.Application.Users.DeactivateUser;

public sealed record DeactivateUserCommand(
    Guid UserId);