namespace Roggy.Application.ListeningHistory.GetListeningHistory;

public sealed record GetListeningHistoryQuery(
    Guid UserId,
    int Limit = 50);