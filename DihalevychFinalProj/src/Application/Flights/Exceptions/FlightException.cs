using Domain.Flights;

namespace Application.Flights.Exceptions;

public abstract class FlightException(FlightId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public FlightId FlightId { get; } = id;
}

public class FlightNotFoundException(FlightId id) : FlightException(id, $"Flight with ID: {id} not found.");

public class FlightAlreadyExistsException(FlightId id) : FlightException(id, $"Flight with ID: {id} already exists.");
public class FlightUnknownException(FlightId id, Exception innerException) : FlightException(id, $"Unknown error occurred for flight with ID: {id}.", innerException);