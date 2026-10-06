using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Albums.DeleteAlbum;

public sealed class DeleteAlbumHandler
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteAlbumHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        DeleteAlbumCommand command,
        CancellationToken cancellationToken = default)
    {
        var album = await _dbContext.Albums
            .FirstOrDefaultAsync(
                album => album.Id == command.AlbumId,
                cancellationToken);

        if (album is null)
        {
            throw new KeyNotFoundException(
                "Album was not found.");
        }

        _dbContext.Albums.Remove(album);

        await _dbContext.SaveChangesAsync(
            cancellationToken);
    }
}