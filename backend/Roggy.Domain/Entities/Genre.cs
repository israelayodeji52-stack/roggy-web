using Roggy.Domain.Common;

namespace Roggy.Domain.Entities;

public class Genre : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    public string Slug { get; private set; } = string.Empty;

    private Genre()
    {
    }

    public Genre(
        string name,
        string slug)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Genre name is required.",
                nameof(name));
        }

        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new ArgumentException(
                "Genre slug is required.",
                nameof(slug));
        }

        Name = name.Trim();
        Slug = slug.Trim().ToLowerInvariant();
    }

    public void Update(
        string name,
        string slug)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Genre name is required.",
                nameof(name));
        }

        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new ArgumentException(
                "Genre slug is required.",
                nameof(slug));
        }

        Name = name.Trim();
        Slug = slug.Trim().ToLowerInvariant();
    }
}