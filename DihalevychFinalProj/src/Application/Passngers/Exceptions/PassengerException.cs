using Domain.Passengers;

namespace Application.Passengers.Exceptions;

public abstract class PassengerException(PassengerId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public PassengerId PassengerId { get; } = id;
}

public class PassengerNotFoundException(PassengerId id) : PassengerException(id, $"Passenger with ID: {id} not found");

public class PassengerAlreadyExistsException(PassengerId id) : PassengerException(id, $"Passenger with ID: {id} already exists");

public class PassengerUnknownException(PassengerId id, Exception innerException) 
    : PassengerException(id, $"An unknown exception occurred for the passenger with ID: {id}", innerException);