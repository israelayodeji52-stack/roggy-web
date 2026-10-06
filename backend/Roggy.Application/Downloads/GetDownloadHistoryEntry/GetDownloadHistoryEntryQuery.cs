namespace Roggy.Application.Downloads.GetDownloadHistoryEntry;

public sealed record GetDownloadHistoryEntryQuery(
    Guid DownloadHistoryId,
    Guid UserId);