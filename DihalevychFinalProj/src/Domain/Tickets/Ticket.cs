using Domain.Flights;
using Domain.Passengers;

namespace Domain.Tickets;

public class Ticket
{
    public TicketId Id { get; }
    public Flight? Flight { get; private set; }
    public FlightId FlightId { get; private set; }
    public Passenger? Passenger { get; private set; }
    public PassengerId PassengerId { get; private set; }
    public DateTime PurchaseDate { get; private set; }
    
    private Ticket(TicketId id, FlightId flightId, PassengerId passengerId,DateTime purchaseDate)
    {
        Id = id;
        FlightId = flightId;
        PassengerId = passengerId;
        PurchaseDate = purchaseDate;
    }

    public static Ticket New(TicketId id, FlightId flightId, PassengerId passengerId)
        =>new(id, flightId, passengerId,DateTime.UtcNow);
    
}