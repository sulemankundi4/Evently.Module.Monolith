namespace Evently.Modules.Events.PublicApi;

public sealed record TicketTypeResponse(
    Guid TicketTypeId,
    string Name,
    decimal Price,
    string Currency,
    decimal Quantity);
