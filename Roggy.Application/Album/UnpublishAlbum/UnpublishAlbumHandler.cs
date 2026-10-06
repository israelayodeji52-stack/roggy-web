using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Albums.UnpublishAlbum;

public sealed class UnpublishAlbumHandler
{
    private readonly IApplicationDbContext _dbContext;

    public UnpublishAlbumHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Album> HandleAsync(
        UnpublishAlbumCommand command,
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

        album.Unpublish();

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        return album;
    }
}