namespace Roggy.Contracts.ListeningHistory;

public sealed record ListeningHistoryResponse(
    Guid Id,
    Guid UserId,
    Guid SongId,
    string SongTitle,
    string? SongCoverImageUrl,
    DateTime StartedAt,
    DateTime? CompletedAt,
    TimeSpan DurationPlayed,
    DateTime CreatedAt,
    DateTime UpdatedAt);