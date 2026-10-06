using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Albums.UpdateAlbum;

public sealed class UpdateAlbumHandler
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateAlbumHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Album> HandleAsync(
        UpdateAlbumCommand command,
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

        album.UpdateAlbum(
            command.Title,
            command.Description,
            command.CoverImageUrl,
            command.ReleaseDate);

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        return album;
    }
}