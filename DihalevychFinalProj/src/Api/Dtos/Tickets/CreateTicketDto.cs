using Domain.Tickets;

namespace Api.Dtos.Tickets;

public record CreateTicketDto(
    Guid FlightId,
    Guid PassengerId)
{
    public static CreateTicketDto FromDomainModel(Ticket ticket)
        => new(
            FlightId: ticket.FlightId.Value,
            PassengerId: ticket.PassengerId.Value);
}