using Domain.Airplanes;
using Domain.Airports;
using Domain.Passengers;

namespace Domain.Flights;

public class Flight
{
    public FlightId Id { get; }
    public string FlightName { get; private set; }
    public DateTime DepartureTime { get; private set; }
    public DateTime ArrivalTime { get; private set; }
    
    public Airport? DepartureAirport { get; private set; }
    public AirportId DepartureAirportId { get; }
    public Airport? ArrivalAirport { get; private set; }
    public AirportId ArrivalAirportId { get; }
    public Airplane? Airplane { get; private set; }
    public AirplaneId AirplaneId { get; private set; }
    

    private Flight(FlightId id, string flightName, DateTime departureTime, DateTime arrivalTime,
        AirportId departureAirportId, AirportId arrivalAirportId,AirplaneId airplaneId)
    {
        Id = id;
        FlightName = flightName;
        DepartureTime = departureTime;
        ArrivalTime = arrivalTime;
        DepartureAirportId = departureAirportId;
        ArrivalAirportId = arrivalAirportId;
        AirplaneId = airplaneId;
    }

    public static Flight New(FlightId id, string flightName, DateTime departureTime, DateTime arrivalTime,
        AirportId departureAirportId, AirportId arrivalAirportId, AirplaneId airplaneId)
        => new(id, flightName, departureTime, arrivalTime, departureAirportId, arrivalAirportId,airplaneId);
    
}