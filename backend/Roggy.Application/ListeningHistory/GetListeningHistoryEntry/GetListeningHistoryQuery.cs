namespace Roggy.Application.ListeningHistory.GetListeningHistoryEntry;

public sealed record GetListeningHistoryEntryQuery(
    Guid HistoryId,
    Guid UserId);