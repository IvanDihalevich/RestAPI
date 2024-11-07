using Domain.Airplanes;
using Domain.Airports;
using Domain.Flights;
using System;

namespace Tests.Data
{
    public static class FlightsData
    {
        public static Flight MainFlight(AirportId departureAirportId, AirportId arrivalAirportId, AirplaneId airplaneId)
            => Flight.New(FlightId.New(), "Test Flight", DateTime.UtcNow.AddHours(1), DateTime.UtcNow.AddHours(5),
                departureAirportId, arrivalAirportId, airplaneId);
    }
}