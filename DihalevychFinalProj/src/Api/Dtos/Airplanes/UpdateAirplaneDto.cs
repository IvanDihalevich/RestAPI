using Domain.Airplanes;

namespace Api.Dtos.Airplanes;

public record UpdateAirplaneDto(
    Guid AirplaneId,
    string Model,
    int MaxPassenger,
    int YearOfManufacture,
    Guid AirportId)
{
    public static UpdateAirplaneDto FromDomainModel(Airplane airplane)
        => new(
            AirplaneId: airplane.Id.Value,
            Model: airplane.Model,
            MaxPassenger: airplane.MaxPassenger,
            YearOfManufacture: airplane.YearOfManufacture,
            AirportId: airplane.AirportId.Value);
}