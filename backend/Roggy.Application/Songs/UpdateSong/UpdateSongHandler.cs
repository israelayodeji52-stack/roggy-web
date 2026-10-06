using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Songs.UpdateSong;

public sealed class UpdateSongHandler
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateSongHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Song?> HandleAsync(
        UpdateSongCommand command,
        CancellationToken cancellationToken = default)
    {
        var song = await _dbContext.Songs
            .FirstOrDefaultAsync(
                song => song.Id == command.SongId,
                cancellationToken);

        if (song is null)
        {
            return null;
        }

        if (command.AlbumId.HasValue)
        {
            var albumExists = await _dbContext.Albums
                .AnyAsync(
                    album =>
                        album.Id == command.AlbumId.Value &&
                        album.ArtistId == song.ArtistId,
                    cancellationToken);

            if (!albumExists)
            {
                throw new KeyNotFoundException(
                    "Album was not found for this artist.");
            }
        }

        song.SetAlbum(command.AlbumId);

        song.UpdateSong(
            command.GenreId,
            command.Title,
            command.Description,
            command.AudioUrl,
            command.CoverImageUrl,
            command.Duration,
            command.ReleaseDate,
            command.ContentRating,
            command.AllowRemixing,
            command.TrackNumber);

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        return song;
    }
}