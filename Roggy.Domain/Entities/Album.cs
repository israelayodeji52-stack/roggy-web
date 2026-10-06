using Roggy.Domain.Common;

namespace Roggy.Domain.Entities;

public class Album : BaseEntity
{
    public Guid ArtistId { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public string? CoverImageUrl { get; private set; }

    public DateTime? ReleaseDate { get; private set; }

    public bool IsPublished { get; private set; }

    public ArtistProfile Artist { get; private set; } = null!;

    public ICollection<Song> Songs { get; private set; } = new List<Song>();

    private Album()
    {
    }

    public Album(
        Guid artistId,
        string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException(
                "Album title is required.",
                nameof(title));

        ArtistId = artistId;
        Title = title.Trim();
        IsPublished = false;
    }

    public void UpdateAlbum(
        string title,
        string? description,
        string? coverImageUrl,
        DateTime? releaseDate)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException(
                "Album title is required.",
                nameof(title));

        Title = title.Trim();
        Description = description?.Trim();
        CoverImageUrl = coverImageUrl?.Trim();
        ReleaseDate = releaseDate;
    }

    public void Publish()
    {
        IsPublished = true;
    }

    public void Unpublish()
    {
        IsPublished = false;
    }
}