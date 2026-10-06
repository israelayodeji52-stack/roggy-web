namespace Roggy.Application.ListeningHistory.RecordListeningHistory;

public sealed record RecordListeningHistoryCommand(
    Guid UserId,
    Guid SongId,
    DateTime StartedAt,
    DateTime? CompletedAt,
    TimeSpan DurationPlayed);