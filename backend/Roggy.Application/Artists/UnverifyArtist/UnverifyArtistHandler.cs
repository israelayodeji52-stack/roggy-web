using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Artists.UnverifyArtist;

public sealed class UnverifyArtistHandler
{
    private readonly IApplicationDbContext _dbContext;

    public UnverifyArtistHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ArtistProfile> HandleAsync(
        UnverifyArtistCommand command,
        CancellationToken cancellationToken = default)
    {
        var artistProfile = await _dbContext.ArtistProfiles
            .FirstOrDefaultAsync(
                artist => artist.Id == command.ArtistId,
                cancellationToken);

        if (artistProfile is null)
        {
            throw new KeyNotFoundException(
                "Artist profile was not found.");
        }

        artistProfile.Unverify();

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        return artistProfile;
    }
}