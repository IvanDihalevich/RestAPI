using Domain.Airports;

namespace Tests.Data;

public static class AirportsData
{
    public static Airport MainAirport()
        => Airport.New(AirportId.New(), "Main Airport", "Main Location");
}