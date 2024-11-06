using Domain.Flights;

namespace Api.Dtos.Flights;

public record UpdateFlightDto(
    string FlightName,
    DateTime DepartureTime,
    DateTime ArrivalTime)
{
    public static UpdateFlightDto FromDomainModel(Flight flight)
        => new(
            FlightName: flight.FlightName,
            DepartureTime: flight.DepartureTime,
            ArrivalTime: flight.ArrivalTime);
}