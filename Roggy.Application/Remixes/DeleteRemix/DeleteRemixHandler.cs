using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Remixes.DeleteRemix;

public sealed class DeleteRemixHandler
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteRemixHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        DeleteRemixCommand command,
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

        _dbContext.Remixes.Remove(remix);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}