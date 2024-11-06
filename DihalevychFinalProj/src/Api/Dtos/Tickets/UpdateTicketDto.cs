using Domain.Tickets;

namespace Api.Dtos.Tickets;

public record UpdateTicketDto(
    Guid FlightId,
    Guid PassengerId)
{
    public static UpdateTicketDto FromDomainModel(Ticket ticket)
        => new(
            FlightId: ticket.FlightId.Value,
            PassengerId: ticket.PassengerId.Value);
}