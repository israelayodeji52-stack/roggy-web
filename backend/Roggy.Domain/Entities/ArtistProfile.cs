using Roggy.Domain.Common;

namespace Roggy.Domain.Entities;

public class ArtistProfile : BaseEntity
{
    public Guid UserId { get; private set; }

    public string StageName { get; private set; } = string.Empty;

    public string? Biography { get; private set; }

    public string? ProfileImageUrl { get; private set; }

    public string? BannerImageUrl { get; private set; }

    public bool IsVerified { get; private set; }

    public User User { get; private set; } = null!;

    private ArtistProfile()
    {
    }

    public ArtistProfile(
        Guid userId,
        string stageName)
    {
        if (string.IsNullOrWhiteSpace(stageName))
            throw new ArgumentException(
                "Stage name is required.",
                nameof(stageName));

        UserId = userId;
        StageName = stageName.Trim();
        IsVerified = false;
    }

    public void UpdateProfile(
        string stageName,
        string? biography,
        string? profileImageUrl,
        string? bannerImageUrl)
    {
        if (string.IsNullOrWhiteSpace(stageName))
            throw new ArgumentException(
                "Stage name is required.",
                nameof(stageName));

        StageName = stageName.Trim();
        Biography = biography?.Trim();
        ProfileImageUrl = profileImageUrl?.Trim();
        BannerImageUrl = bannerImageUrl?.Trim();

        UpdateTimestamp();
    }

    public void Verify()
    {
        IsVerified = true;
        UpdateTimestamp();
    }

    public void Unverify()
    {
        IsVerified = false;
        UpdateTimestamp();
    }
}