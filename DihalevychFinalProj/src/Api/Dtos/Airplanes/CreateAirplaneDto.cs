using Api.Dtos.Airports;
using Domain.Airports;
using Domain.Airplanes;

namespace Api.Dtos.Airplanes;

public record CreateAirplaneDto(
    string Model,
    int MaxPassenger,
    int YearOfManufacture,
    Guid AirportId)
{
    public static CreateAirplaneDto FromDomainModel(Airplane airplane)
        => new(
            Model: airplane.Model,
            MaxPassenger: airplane.MaxPassenger,
            YearOfManufacture: airplane.YearOfManufacture,
            AirportId: airplane.AirportId.Value);
}