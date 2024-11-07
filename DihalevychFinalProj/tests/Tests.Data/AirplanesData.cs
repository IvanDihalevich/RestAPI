using Domain.Airplanes;
using Domain.Airports;

namespace Tests.Data
{
    public static class AirplanesData
    {

        public static Airplane MainAirplane(AirportId airportId)
            => Airplane.New(AirplaneId.New(), "Test Airplane", 150, 2010, airportId);

    }
}