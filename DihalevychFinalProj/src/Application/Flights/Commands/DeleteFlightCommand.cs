using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Flights.Exceptions;
using Domain.Flights;
using MediatR;

namespace Application.Flights.Commands;

public record DeleteFlightCommand : IRequest<Result<Flight, FlightException>>
{
    public required Guid FlightId { get; init; }
}

public class DeleteFlightCommandHandler(IFlightRepository flightRepository)
    : IRequestHandler<DeleteFlightCommand, Result<Flight, FlightException>>
{
    public async Task<Result<Flight, FlightException>> Handle(DeleteFlightCommand request, CancellationToken cancellationToken)
    {
        var flightId = new FlightId(request.FlightId);

        var existingFlight = await flightRepository.GetById(flightId, cancellationToken);

        return await existingFlight.Match(
            async f => await DeleteEntity(f, cancellationToken),
            () => Task.FromResult<Result<Flight, FlightException>>(new FlightNotFoundException(flightId)));
    }

    private async Task<Result<Flight, FlightException>> DeleteEntity(Flight flight, CancellationToken cancellationToken)
    {
        try
        {
            return await flightRepository.Delete(flight, cancellationToken);
        }
        catch (Exception ex)
        {
            return new FlightUnknownException(flight.Id, ex);
        }
    }
}