using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Remixes.GetSongRemixes;

public sealed class GetSongRemixesHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetSongRemixesHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Roggy.Domain.Entities.Remix>>
        HandleAsync(
            GetSongRemixesQuery query,
            CancellationToken cancellationToken = default)
    {
        if (query.OriginalSongId == Guid.Empty)
        {
            throw new ArgumentException(
                "Original song ID is required.",
                nameof(query.OriginalSongId));
        }

        var limit = query.Limit <= 0
            ? 50
            : Math.Min(query.Limit, 100);

        return await _dbContext.Remixes
            .AsNoTracking()
            .Include(remix => remix.OriginalSong)
            .Include(remix => remix.RemixerArtist)
            .Where(remix =>
                remix.OriginalSongId == query.OriginalSongId &&
                remix.IsPublished)
            .OrderByDescending(remix => remix.CreatedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}