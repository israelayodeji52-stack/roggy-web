using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.ContentPreferences.GetContentPreference;

public sealed class GetContentPreferenceHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetContentPreferenceHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ContentPreference> HandleAsync(
        GetContentPreferenceQuery query,
        CancellationToken cancellationToken = default)
    {
        if (query.UserId == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(query.UserId));
        }

        var preference = await _dbContext.ContentPreferences
            .FirstOrDefaultAsync(
                item => item.UserId == query.UserId,
                cancellationToken);

        if (preference is null)
        {
            var userExists = await _dbContext.Users
                .AnyAsync(
                    user => user.Id == query.UserId,
                    cancellationToken);

            if (!userExists)
            {
                throw new KeyNotFoundException(
                    "User was not found.");
            }

            preference = new ContentPreference(query.UserId);

            _dbContext.ContentPreferences.Add(preference);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return preference;
    }
}