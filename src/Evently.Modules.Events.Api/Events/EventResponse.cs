namespace Evently.Modules.Events.Api.Events;

internal sealed record EventResponse(
    string Title,
    string Description,
    string Location,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc
);
