using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Notifications.CreateNotification;

public sealed class CreateNotificationHandler
{
    private readonly IApplicationDbContext _dbContext;

    public CreateNotificationHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Notification> HandleAsync(
        CreateNotificationCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.UserId == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(command.UserId));
        }

        if (string.IsNullOrWhiteSpace(command.Title))
        {
            throw new ArgumentException(
                "Notification title is required.",
                nameof(command.Title));
        }

        if (string.IsNullOrWhiteSpace(command.Message))
        {
            throw new ArgumentException(
                "Notification message is required.",
                nameof(command.Message));
        }

        var userExists = await _dbContext.Users
            .FindAsync(
                [command.UserId],
                cancellationToken);

        if (userExists is null)
        {
            throw new KeyNotFoundException(
                "User was not found.");
        }

        var notification = new Notification(
            command.UserId,
            command.Title,
            command.Message,
            command.ActionUrl);

        _dbContext.Notifications.Add(notification);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return notification;
    }
}