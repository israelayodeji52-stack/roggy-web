using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Artists.VerifyArtist;

public sealed class VerifyArtistHandler
{
    private readonly IApplicationDbContext _dbContext;

    public VerifyArtistHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ArtistProfile> HandleAsync(
        VerifyArtistCommand command,
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

        artistProfile.Verify();

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        return artistProfile;
    }
}