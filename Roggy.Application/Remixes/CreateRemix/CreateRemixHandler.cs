using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Remixes.CreateRemix;

public sealed class CreateRemixHandler
{
    private readonly IApplicationDbContext _dbContext;

    public CreateRemixHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Remix> HandleAsync(
        CreateRemixCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.UserId == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(command.UserId));
        }

        if (command.OriginalSongId == Guid.Empty)
        {
            throw new ArgumentException(
                "Original song ID is required.",
                nameof(command.OriginalSongId));
        }

        if (string.IsNullOrWhiteSpace(command.Title))
        {
            throw new ArgumentException(
                "Remix title is required.",
                nameof(command.Title));
        }

        if (string.IsNullOrWhiteSpace(command.AudioUrl))
        {
            throw new ArgumentException(
                "Remix audio URL is required.",
                nameof(command.AudioUrl));
        }

        if (command.Duration <= TimeSpan.Zero)
        {
            throw new ArgumentException(
                "Remix duration must be greater than zero.",
                nameof(command.Duration));
        }

        var artist = await _dbContext.ArtistProfiles
            .FirstOrDefaultAsync(
                artist =>
                    artist.UserId == command.UserId,
                cancellationToken);

        if (artist is null)
        {
            throw new KeyNotFoundException(
                "Artist profile was not found.");
        }

        var originalSong = await _dbContext.Songs
            .FirstOrDefaultAsync(
                song =>
                    song.Id == command.OriginalSongId &&
                    song.IsPublished,
                cancellationToken);

        if (originalSong is null)
        {
            throw new KeyNotFoundException(
                "Published original song was not found.");
        }

        if (!originalSong.AllowRemixing)
        {
            throw new InvalidOperationException(
                "This song does not allow remixing.");
        }

        var remix = new Remix(
            command.OriginalSongId,
            artist.Id,
            command.Title,
            command.AudioUrl,
            command.Duration);

        remix.Update(
            command.Title,
            command.AudioUrl,
            command.CoverImageUrl,
            command.Duration,
            Roggy.Domain.Enums.ContentRating.Clean);

        _dbContext.Remixes.Add(remix);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return await _dbContext.Remixes
            .Include(remix => remix.OriginalSong)
            .Include(remix => remix.RemixerArtist)
            .FirstAsync(
                remix => remix.Id == remix.Id,
                cancellationToken);
    }
}