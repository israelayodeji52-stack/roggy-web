namespace Roggy.Application.ContentPreferences.UpdateContentPreference;

public sealed record UpdateContentPreferenceCommand(
    Guid UserId,
    bool AllowExplicitMusic,
    bool AllowExplicitPodcasts,
    bool AllowMatureContent);