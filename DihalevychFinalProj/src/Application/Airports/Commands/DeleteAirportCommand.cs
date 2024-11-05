using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Airports.Exceptions;
using Domain.Airports;
using MediatR;

namespace Application.Airports.Commands;

public record DeleteAirportCommand : IRequest<Result<Airport, AirportException>>
{
    public required Guid AirportId { get; init; }
}

public class DeleteAirportCommandHandler(IAirportRepository airportRepository)
    : IRequestHandler<DeleteAirportCommand, Result<Airport, AirportException>>
{
    public async Task<Result<Airport, AirportException>> Handle(
        DeleteAirportCommand request,
        CancellationToken cancellationToken)
    {
        var airportId = new AirportId(request.AirportId);

        var existingAirport = await airportRepository.GetById(airportId, cancellationToken);

        return await existingAirport.Match<Task<Result<Airport, AirportException>>>(
            async a => await DeleteEntity(a, cancellationToken),
            () => Task.FromResult<Result<Airport, AirportException>>(new AirportNotFoundException(airportId)));
    }

    public async Task<Result<Airport, AirportException>> DeleteEntity(Airport airport, CancellationToken cancellationToken)
    {
        try
        {
            return await airportRepository.Delete(airport, cancellationToken);
        }
        catch (Exception exception)
        {
            return new AirportUnknownException(airport.Id, exception);
        }
    }
}