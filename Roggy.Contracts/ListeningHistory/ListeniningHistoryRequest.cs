namespace Roggy.Contracts.ListeningHistory;

public sealed record RecordListeningHistoryRequest(
    Guid SongId,
    DateTime StartedAt,
    DateTime? CompletedAt,
    TimeSpan DurationPlayed);