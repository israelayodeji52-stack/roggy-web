using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roggy.Application.ContentPreferences.GetContentPreference;
using Roggy.Application.ContentPreferences.UpdateContentPreference;
using Roggy.Contracts.ContentPreferences;
using Roggy.Domain.Entities;

namespace Roggy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContentPreferencesController : ControllerBase
{
    private readonly GetContentPreferenceHandler _getContentPreferenceHandler;
    private readonly UpdateContentPreferenceHandler _updateContentPreferenceHandler;

    public ContentPreferencesController(
        GetContentPreferenceHandler getContentPreferenceHandler,
        UpdateContentPreferenceHandler updateContentPreferenceHandler)
    {
        _getContentPreferenceHandler = getContentPreferenceHandler;
        _updateContentPreferenceHandler = updateContentPreferenceHandler;
    }

    [HttpGet]
    public async Task<ActionResult<ContentPreferenceResponse>> Get(
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            var preference =
                await _getContentPreferenceHandler.HandleAsync(
                    new GetContentPreferenceQuery(userId),
                    cancellationToken);

            return Ok(ToResponse(preference));
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

    [HttpPut]
    public async Task<ActionResult<ContentPreferenceResponse>> Update(
        [FromBody] ContentPreferenceResponse request,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            var preference =
                await _updateContentPreferenceHandler.HandleAsync(
                    new UpdateContentPreferenceCommand(
                        userId,
                        request.AllowExplicitMusic,
                        request.AllowExplicitPodcasts,
                        request.AllowMatureContent),
                    cancellationToken);

            return Ok(ToResponse(preference));
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

    private static ContentPreferenceResponse ToResponse(
        ContentPreference preference)
    {
        return new ContentPreferenceResponse(
            preference.UserId,
            preference.AllowExplicitMusic,
            preference.AllowExplicitPodcasts,
            preference.AllowMatureContent,
            preference.UpdatedAt);
    }
}