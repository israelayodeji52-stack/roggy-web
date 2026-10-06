using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Artists.CreateArtistProfile;

public sealed class CreateArtistProfileHandler
{
    private readonly IApplicationDbContext _dbContext;

    public CreateArtistProfileHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ArtistProfile> HandleAsync(
        CreateArtistProfileCommand command,
        CancellationToken cancellationToken = default)
    {
        var stageName = command.StageName.Trim();

        if (string.IsNullOrWhiteSpace(stageName))
        {
            throw new ArgumentException(
                "Stage name is required.",
                nameof(command.StageName));
        }

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(
                user => user.Id == command.UserId,
                cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException(
                "User was not found.");
        }

        if (!user.IsActive)
        {
            throw new InvalidOperationException(
                "The user account is inactive.");
        }

        var existingArtistProfile =
            await _dbContext.ArtistProfiles
                .AnyAsync(
                    artist => artist.UserId == command.UserId,
                    cancellationToken);

        if (existingArtistProfile)
        {
            throw new InvalidOperationException(
                "This user already has an artist profile.");
        }

        var stageNameExists =
            await _dbContext.ArtistProfiles
                .AnyAsync(
                    artist => artist.StageName == stageName,
                    cancellationToken);

        if (stageNameExists)
        {
            throw new InvalidOperationException(
                "This stage name is already taken.");
        }

        var artistProfile = new ArtistProfile(
            command.UserId,
            stageName);

        _dbContext.ArtistProfiles.Add(artistProfile);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return artistProfile;
    }
}