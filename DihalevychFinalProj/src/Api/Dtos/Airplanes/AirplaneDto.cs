using Api.Dtos.Airports;
using Domain.Airports;
using Domain.Airplanes;

namespace Api.Dtos.Airplanes;

public record AirplaneDto(
    Guid? Id,
    string Model,
    int MaxPassenger,
    int YearOfManufacture,
    Guid AirportId,
    AirportDto? Airport)
{
    public static AirplaneDto FromDomainModel(Airplane airplane)
        => new(
            Id: airplane.Id.Value,
            Model: airplane.Model,
            MaxPassenger: airplane.MaxPassenger,
            YearOfManufacture: airplane.YearOfManufacture,
            AirportId: airplane.AirportId.Value,
            Airport: airplane.Airport == null ? null : AirportDto.FromDomainModel(airplane.Airport));
}