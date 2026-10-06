using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roggy.Application.Genres.CreateGenre;
using Roggy.Contracts.Genres;

namespace Roggy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenresController : ControllerBase
{
    private readonly CreateGenreHandler _createGenreHandler;

    public GenresController(
        CreateGenreHandler createGenreHandler)
    {
        _createGenreHandler = createGenreHandler;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<GenreResponse>> Create(
        [FromBody] CreateGenreRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateGenreCommand(
                request.Name,
                request.Slug);

            var genre = await _createGenreHandler.HandleAsync(
                command,
                cancellationToken);

            var response = new GenreResponse(
                genre.Id,
                genre.Name,
                genre.Slug,
                genre.CreatedAt,
                genre.UpdatedAt);

            return Created(
                $"/api/Genres/{genre.Id}",
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
}