using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Airports.Exceptions;
using Domain.Airports;
using MediatR;

namespace Application.Airports.Commands;

public record UpdateAirportCommand : IRequest<Result<Airport, AirportException>>
{
    public required Guid AirportId { get; init; }
    public required string Name { get; init; }
    public required string Location { get; init; }
}

public class UpdateAirportCommandHandler(IAirportRepository airportRepository) 
    : IRequestHandler<UpdateAirportCommand, Result<Airport, AirportException>>
{
    public async Task<Result<Airport, AirportException>> Handle(UpdateAirportCommand request, CancellationToken cancellationToken)
    {
        var airportId = new AirportId(request.AirportId);

        var existingAirport = await airportRepository.GetById(airportId, cancellationToken);

        return await existingAirport.Match(
            async a => await UpdateEntity(a, request.Name, request.Location, cancellationToken),
            () => Task.FromResult<Result<Airport, AirportException>>(new AirportNotFoundException(airportId)));
    }

    private async Task<Result<Airport, AirportException>> UpdateEntity(
        Airport entity,
        string name,
        string location,
        CancellationToken cancellationToken)
    {
        try
        {
            entity.UpdateDetails(name, location);

            return await airportRepository.Update(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new AirportUnknownException(entity.Id, exception);
        }
    }
}