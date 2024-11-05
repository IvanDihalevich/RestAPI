using Domain.Airports;

namespace Application.Airports.Exceptions;

public abstract class AirportException(AirportId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public AirportId AirportId { get; } = id;
}

public class AirportNotFoundException(AirportId id) 
    : AirportException(id, $"Airport with ID: {id} not found");

public class AirportAlreadyExistsException(AirportId id) 
    : AirportException(id, $"Airport with ID: {id} already exists");

public class AirportUnknownException(AirportId id, Exception innerException) 
    : AirportException(id, $"An unknown exception occurred for the airport with ID: {id}", innerException);