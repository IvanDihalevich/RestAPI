using Domain.Flights;
using Domain.Passengers;
using Domain.Tickets;

namespace Tests.Data
{
    public static class TicketsData
    {
        public static Ticket MainTicket(FlightId flightId, PassengerId passengerId)
            => Ticket.New(TicketId.New(), flightId, passengerId);
    }
}