using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.ContentPreferences.UpdateContentPreference;

public sealed class UpdateContentPreferenceHandler
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateContentPreferenceHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ContentPreference> HandleAsync(
        UpdateContentPreferenceCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.UserId == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(command.UserId));
        }

        var preference = await _dbContext.ContentPreferences
            .FirstOrDefaultAsync(
                item => item.UserId == command.UserId,
                cancellationToken);

        if (preference is null)
        {
            var userExists = await _dbContext.Users
                .AnyAsync(
                    user => user.Id == command.UserId,
                    cancellationToken);

            if (!userExists)
            {
                throw new KeyNotFoundException(
                    "User was not found.");
            }

            preference = new ContentPreference(command.UserId);

            _dbContext.ContentPreferences.Add(preference);
        }

        preference.Update(
            command.AllowExplicitMusic,
            command.AllowExplicitPodcasts,
            command.AllowMatureContent);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return preference;
    }
}