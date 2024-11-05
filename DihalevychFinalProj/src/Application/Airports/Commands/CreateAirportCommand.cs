using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Airports.Exceptions;
using Domain.Airports;
using MediatR;

namespace Application.Airports.Commands;

public record CreateAirportCommand : IRequest<Result<Airport, AirportException>>
{
    public required string Name { get; init; }
    public required string Location { get; init; }
}

public class CreateAirportCommandHandler(IAirportRepository airportRepository)
    : IRequestHandler<CreateAirportCommand, Result<Airport, AirportException>>
{
    public async Task<Result<Airport, AirportException>> Handle(CreateAirportCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = Airport.New(AirportId.New(), request.Name, request.Location);

            return await airportRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new AirportUnknownException(AirportId.Empty(), exception);
        }
    }
}