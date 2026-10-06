namespace Roggy.Contracts.ContentPreferences;

public sealed record ContentPreferenceResponse(
    Guid UserId,
    bool AllowExplicitMusic,
    bool AllowExplicitPodcasts,
    bool AllowMatureContent,
    DateTime UpdatedAt);