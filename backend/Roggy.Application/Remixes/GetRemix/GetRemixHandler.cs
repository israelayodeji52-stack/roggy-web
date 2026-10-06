using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Remixes.GetRemix;

public sealed class GetRemixHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetRemixHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Roggy.Domain.Entities.Remix?> HandleAsync(
        GetRemixQuery query,
        CancellationToken cancellationToken = default)
    {
        if (query.RemixId == Guid.Empty)
        {
            throw new ArgumentException(
                "Remix ID is required.",
                nameof(query.RemixId));
        }

        return await _dbContext.Remixes
            .AsNoTracking()
            .Include(remix => remix.OriginalSong)
            .Include(remix => remix.RemixerArtist)
            .FirstOrDefaultAsync(
                remix =>
                    remix.Id == query.RemixId &&
                    remix.IsPublished,
                cancellationToken);
    }
}