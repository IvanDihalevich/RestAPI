using Domain.Airports;

namespace Api.Dtos.Airports;


public record AirportDto(
    Guid? Id,
    string Name,
    string Location)
{
    public static AirportDto FromDomainModel(Airport airport)
        => new(
            Id: airport.Id.Value,
            Name: airport.Name,
            Location: airport.Location);
}
