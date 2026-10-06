using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Remixes.UnpublishRemix;

public sealed class UnpublishRemixHandler
{
    private readonly IApplicationDbContext _dbContext;

    public UnpublishRemixHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Roggy.Domain.Entities.Remix> HandleAsync(
        UnpublishRemixCommand command,
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

        remix.Unpublish();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return remix;
    }
}