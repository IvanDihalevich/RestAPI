using Domain.Airplanes;
using Domain.Airports;

namespace Application.Airplanes.Exceptions;

public abstract class AirplaneException(AirplaneId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public AirplaneId AirplaneId { get; } = id;
}

public class AirplaneNotFoundException(AirplaneId id) : AirplaneException(id, $"Airplane under id: {id} not found");

public class AirplaneAlreadyExistsException(AirplaneId id) : AirplaneException(id, $"Airplane already exists: {id}");

public class AirportNotFoundException(AirportId airportId) : AirplaneException(AirplaneId.Empty(), $"Airport under id: {airportId} not found");

public class AirplaneUnknownException(AirplaneId id, Exception innerException)
    : AirplaneException(id, $"Unknown exception for the airplane under id: {id}", innerException);