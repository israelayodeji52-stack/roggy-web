using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roggy.Application.Notifications.CreateNotification;
using Roggy.Application.Notifications.GetNotifications;
using Roggy.Application.Notifications.MarkNotificationAsRead;
using Roggy.Contracts.Notifications;

namespace Roggy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly CreateNotificationHandler _createNotificationHandler;
    private readonly GetNotificationsHandler _getNotificationsHandler;
    private readonly MarkNotificationAsReadHandler _markNotificationAsReadHandler;

    public NotificationController(
        CreateNotificationHandler createNotificationHandler,
        GetNotificationsHandler getNotificationsHandler,
        MarkNotificationAsReadHandler markNotificationAsReadHandler)
    {
        _createNotificationHandler = createNotificationHandler;
        _getNotificationsHandler = getNotificationsHandler;
        _markNotificationAsReadHandler = markNotificationAsReadHandler;
    }

    [HttpPost]
    public async Task<ActionResult<NotificationResponse>> CreateNotification(
        [FromBody] CreateNotificationRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            var notification =
                await _createNotificationHandler.HandleAsync(
                    new CreateNotificationCommand(
                        userId,
                        request.Title,
                        request.Message,
                        request.ActionUrl),
                    cancellationToken);

            return Ok(ToResponse(notification));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message
            });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<NotificationResponse>>> GetNotifications(
        [FromQuery] int limit = 50,
        CancellationToken cancellationToken = default)
    {
        var userId = GetCurrentUserId();

        try
        {
            var notifications =
                await _getNotificationsHandler.HandleAsync(
                    new GetNotificationsQuery(
                        userId,
                        limit),
                    cancellationToken);

            var response = notifications
                .Select(ToResponse)
                .ToList();

            return Ok(response);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
    }

    [HttpPatch("{notificationId:guid}/read")]
    public async Task<ActionResult<NotificationResponse>> MarkAsRead(
        Guid notificationId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            var notification =
                await _markNotificationAsReadHandler.HandleAsync(
                    new MarkNotificationAsReadCommand(
                        userId,
                        notificationId),
                    cancellationToken);

            return Ok(ToResponse(notification));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message
            });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
    }

    private Guid GetCurrentUserId()
    {
        var userIdValue =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            throw new UnauthorizedAccessException(
                "Authenticated user ID is missing.");
        }

        return userId;
    }

    private static NotificationResponse ToResponse(
        Roggy.Domain.Entities.Notification notification)
    {
        return new NotificationResponse(
            notification.Id,
            notification.UserId,
            notification.Title,
            notification.Message,
            notification.ActionUrl,
            notification.IsRead,
            notification.ReadAt,
            notification.CreatedAt,
            notification.UpdatedAt);
    }
}