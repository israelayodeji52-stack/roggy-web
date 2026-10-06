namespace Roggy.Application.Downloads.DeleteDownloadHistoryEntry;

public sealed record DeleteDownloadHistoryEntryCommand(
    Guid DownloadHistoryId,
    Guid UserId);