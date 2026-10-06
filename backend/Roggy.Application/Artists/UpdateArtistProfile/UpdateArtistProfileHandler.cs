using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Artists.UpdateArtistProfile;

public sealed class UpdateArtistProfileHandler
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateArtistProfileHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Roggy.Domain.Entities.ArtistProfile> HandleAsync(
        UpdateArtistProfileCommand command,
        CancellationToken cancellationToken = default)
    {
        var artistProfile = await _dbContext.ArtistProfiles
            .FirstOrDefaultAsync(
                artist => artist.Id == command.ArtistProfileId,
                cancellationToken);

        if (artistProfile is null)
        {
            throw new KeyNotFoundException(
                "Artist profile was not found.");
        }

        var stageName = command.StageName.Trim();

        if (string.IsNullOrWhiteSpace(stageName))
        {
            throw new ArgumentException(
                "Stage name is required.",
                nameof(command.StageName));
        }

        var stageNameExists = await _dbContext.ArtistProfiles
            .AnyAsync(
                artist =>
                    artist.StageName == stageName &&
                    artist.Id != command.ArtistProfileId,
                cancellationToken);

        if (stageNameExists)
        {
            throw new InvalidOperationException(
                "This stage name is already taken.");
        }

        artistProfile.UpdateProfile(
            stageName,
            command.Biography,
            command.ProfileImageUrl,
            command.BannerImageUrl);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return artistProfile;
    }
}