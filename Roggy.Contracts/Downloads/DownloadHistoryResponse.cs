namespace Roggy.Contracts.Downloads;

public sealed record DownloadHistoryResponse(
    Guid Id,
    Guid UserId,
    Guid SongId,
    string SongTitle,
    string? SongCoverImageUrl,
    DateTime DownloadedAt,
    DateTime CreatedAt,
    DateTime UpdatedAt);