using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Remixes.UpdateRemix;

public sealed class UpdateRemixHandler
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateRemixHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Roggy.Domain.Entities.Remix> HandleAsync(
        UpdateRemixCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.RemixId == Guid.Empty)
        {
            throw new ArgumentException(
                "Remix ID is required.",
                nameof(command.RemixId));
        }

        if (command.UserId == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(command.UserId));
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

        var remix = await _dbContext.Remixes
            .Include(remix => remix.OriginalSong)
            .Include(remix => remix.RemixerArtist)
            .FirstOrDefaultAsync(
                remix =>
                    remix.Id == command.RemixId &&
                    remix.RemixerArtist.UserId == command.UserId,
                cancellationToken);

        if (remix is null)
        {
            throw new KeyNotFoundException(
                "Remix was not found.");
        }

        remix.Update(
            command.Title,
            command.AudioUrl,
            command.CoverImageUrl,
            command.Duration,
            command.ContentRating);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return remix;
    }
}