using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roggy.Application.Users.ActivateUser;
using Roggy.Application.Users.CreateUser;
using Roggy.Application.Users.DeactivateUser;
using Roggy.Contracts.Users;

namespace Roggy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly CreateUserHandler _createUserHandler;
    private readonly DeactivateUserHandler _deactivateUserHandler;
    private readonly ActivateUserHandler _activateUserHandler;

    public UsersController(
        CreateUserHandler createUserHandler,
        DeactivateUserHandler deactivateUserHandler,
        ActivateUserHandler activateUserHandler)
    {
        _createUserHandler = createUserHandler;
        _deactivateUserHandler = deactivateUserHandler;
        _activateUserHandler = activateUserHandler;
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> CreateUser(
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateUserCommand(
                request.Username,
                request.Email,
                request.Password,
                request.DisplayName);

            var user = await _createUserHandler.HandleAsync(
                command,
                cancellationToken);

            var response = new UserResponse(
                user.Id,
                user.Username,
                user.Email,
                user.DisplayName);

            return Created(
                $"/api/users/{user.Id}",
                response);
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new
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

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeactivateUser(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            await _deactivateUserHandler.HandleAsync(
                new DeactivateUserCommand(id),
                cancellationToken);

            return NoContent();
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message
            });
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new
            {
                message = exception.Message
            });
        }
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> ActivateUser(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            await _activateUserHandler.HandleAsync(
                new ActivateUserCommand(id),
                cancellationToken);

            return NoContent();
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message
            });
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new
            {
                message = exception.Message
            });
        }
    }
}