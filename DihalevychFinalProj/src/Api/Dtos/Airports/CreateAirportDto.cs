using Domain.Airports;

namespace Api.Dtos.Airports;

public record CreateAirportDto(
    string Name,
    string Location)
{
    public static CreateAirportDto FromDomainModel(Airport airport)
        => new(
            Name: airport.Name,
            Location: airport.Location);
}