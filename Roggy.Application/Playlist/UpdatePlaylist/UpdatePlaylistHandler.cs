using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using PlaylistEntity = Roggy.Domain.Entities.Playlist;

namespace Roggy.Application.Playlist.UpdatePlaylist;

public sealed class UpdatePlaylistHandler
{
    private readonly IApplicationDbContext _dbContext;

    public UpdatePlaylistHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PlaylistEntity> HandleAsync(
        UpdatePlaylistCommand command,
        CancellationToken cancellationToken = default)
    {
        var playlist = await _dbContext.Playlists
            .FirstOrDefaultAsync(
                playlist =>
                    playlist.Id == command.PlaylistId &&
                    playlist.UserId == command.UserId,
                cancellationToken);

        if (playlist is null)
        {
            throw new KeyNotFoundException(
                "Playlist was not found.");
        }

        playlist.Update(
            command.Name,
            command.Description,
            command.CoverImageUrl);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return playlist;
    }
}