namespace Roggy.Application.Downloads.GetDownloadHistory;

public sealed record GetDownloadHistoryQuery(
    Guid UserId,
    int Limit = 50);