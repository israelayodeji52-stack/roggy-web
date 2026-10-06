using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Artists.GetArtistProfile;

public sealed class GetArtistProfileHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetArtistProfileHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ArtistProfile> HandleAsync(
        GetArtistProfileQuery query,
        CancellationToken cancellationToken = default)
    {
        var artistProfile = await _dbContext.ArtistProfiles
            .FirstOrDefaultAsync(
                artist => artist.Id == query.ArtistProfileId,
                cancellationToken);

        if (artistProfile is null)
        {
            throw new KeyNotFoundException(
                "Artist profile was not found.");
        }

        return artistProfile;
    }
}