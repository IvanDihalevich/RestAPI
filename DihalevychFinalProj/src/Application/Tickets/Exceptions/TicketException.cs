using Domain.Tickets;

namespace Application.Tickets.Exceptions;

public abstract class TicketException(TicketId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public TicketId TicketId { get; } = id;
}

public class TicketNotFoundException(TicketId id) : TicketException(id, $"Ticket with ID: {id} was not found");

public class TicketUnknownException(TicketId id, Exception innerException) : TicketException(id, $"An unknown error occurred for the ticket with ID: {id}", innerException);
