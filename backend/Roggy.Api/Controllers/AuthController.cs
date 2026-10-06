using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Application.Users.Login;
using Roggy.Contracts.Users;

namespace Roggy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly LoginHandler _loginHandler;
    private readonly IApplicationDbContext _dbContext;

    public AuthController(
        LoginHandler loginHandler,
        IApplicationDbContext dbContext)
    {
        _loginHandler = loginHandler;
        _dbContext = dbContext;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new LoginCommand(
                request.Email,
                request.Password);

            var result = await _loginHandler.HandleAsync(
                command,
                cancellationToken);

            var response = new LoginResponse(
                result.UserId,
                result.Username,
                result.Email,
                result.DisplayName,
                result.Token);

            return Ok(response);
        }
        catch (UnauthorizedAccessException exception)
        {
            return Unauthorized(new
            {
                message = exception.Message
            });
        }
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<CurrentUserResponse>> Me(
        CancellationToken cancellationToken)
    {
        var userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return Unauthorized(new
            {
                message = "Invalid user identity."
            });
        }

        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                user => user.Id == userId,
                cancellationToken);

        if (user is null)
        {
            return NotFound(new
            {
                message = "User was not found."
            });
        }

        if (!user.IsActive)
        {
            return Unauthorized(new
            {
                message = "This account is inactive."
            });
        }

        var response = new CurrentUserResponse(
            user.Id,
            user.Username,
            user.Email,
            user.DisplayName,
            user.ProfileImageUrl,
            user.Bio,
            user.Role,
            user.IsActive);

        return Ok(response);
    }
}