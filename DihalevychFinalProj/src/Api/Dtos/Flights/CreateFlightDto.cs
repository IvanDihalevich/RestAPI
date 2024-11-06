using Domain.Flights;

namespace Api.Dtos.Flights;

public record CreateFlightDto(
    string FlightName,
    DateTime DepartureTime,
    DateTime ArrivalTime,
    Guid DepartureAirportId,
    Guid ArrivalAirportId,
    Guid AirplaneId)
{
    public static CreateFlightDto FromDomainModel(Flight flight)
        => new(
            FlightName: flight.FlightName,
            DepartureTime: flight.DepartureTime,
            ArrivalTime: flight.ArrivalTime,
            DepartureAirportId: flight.DepartureAirportId.Value,
            ArrivalAirportId: flight.ArrivalAirportId.Value,
            AirplaneId: flight.AirplaneId.Value);
}