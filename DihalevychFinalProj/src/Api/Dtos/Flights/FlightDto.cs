using Domain.Flights;

namespace Api.Dtos.Flights;


public record FlightDto(
    Guid? Id,
    string FlightName,
    DateTime DepartureTime,
    DateTime ArrivalTime,
    Guid DepartureAirportId,
    Guid ArrivalAirportId,
    Guid AirplaneId)
{
    public static FlightDto FromDomainModel(Flight flight)
        => new(
            Id: flight.Id.Value,
            FlightName: flight.FlightName,
            DepartureTime: flight.DepartureTime,
            ArrivalTime: flight.ArrivalTime,
            DepartureAirportId: flight.DepartureAirportId.Value,
            ArrivalAirportId: flight.ArrivalAirportId.Value,
            AirplaneId: flight.AirplaneId.Value);
}
