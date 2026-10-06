using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using PlaylistEntity = Roggy.Domain.Entities.Playlist;

namespace Roggy.Application.Playlist.CreatePlaylist;

public sealed class CreatePlaylistHandler
{
    private readonly IApplicationDbContext _dbContext;

    public CreatePlaylistHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PlaylistEntity> HandleAsync(
        CreatePlaylistCommand command,
        CancellationToken cancellationToken = default)
    {
        var userExists = await _dbContext.Users
            .Where(user =>
                user.Id == command.UserId &&
                user.IsActive)
            .CountAsync(cancellationToken) > 0;

        if (!userExists)
        {
            throw new KeyNotFoundException(
                "Active user was not found.");
        }

        var playlist = new PlaylistEntity(
            command.UserId,
            command.Name);

        _dbContext.Playlists.Add(playlist);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return playlist;
    }
}