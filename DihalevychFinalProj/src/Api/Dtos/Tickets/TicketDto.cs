using Domain.Tickets;

namespace Api.Dtos.Tickets;

public record TicketDto(
    Guid? Id,
    Guid FlightId,
    Guid PassengerId,
    DateTime PurchaseDate)
{
    public static TicketDto FromDomainModel(Ticket ticket)
        => new(
            Id: ticket.Id.Value,
            FlightId: ticket.FlightId.Value,
            PassengerId: ticket.PassengerId.Value,
            PurchaseDate: ticket.PurchaseDate);
}
