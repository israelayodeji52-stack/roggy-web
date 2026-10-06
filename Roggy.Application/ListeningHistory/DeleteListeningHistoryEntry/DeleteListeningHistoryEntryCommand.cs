namespace Roggy.Application.ListeningHistory.DeleteListeningHistoryEntry;

public sealed record DeleteListeningHistoryEntryCommand(
    Guid HistoryId,
    Guid UserId);