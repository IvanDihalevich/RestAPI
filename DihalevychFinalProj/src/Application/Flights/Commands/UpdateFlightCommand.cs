using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Flights.Exceptions;
using Domain.Flights;
using MediatR;

namespace Application.Flights.Commands;

public record UpdateFlightCommand : IRequest<Result<Flight, FlightException>>
{
    public required Guid FlightId { get; init; }
    public required string FlightName { get; init; }
    public required DateTime DepartureTime { get; init; }
    public required DateTime ArrivalTime { get; init; }
}

public class UpdateFlightCommandHandler(IFlightRepository flightRepository)
    : IRequestHandler<UpdateFlightCommand, Result<Flight, FlightException>>
{
    public async Task<Result<Flight, FlightException>> Handle(UpdateFlightCommand request, CancellationToken cancellationToken)
    {
        var flightId = new FlightId(request.FlightId);
        var flight = await flightRepository.GetById(flightId, cancellationToken);

        return await flight.Match(
            async f => await UpdateEntity(f, request.FlightName, request.DepartureTime, request.ArrivalTime, cancellationToken),
            () => Task.FromResult<Result<Flight, FlightException>>(new FlightNotFoundException(flightId)));
    }

    private async Task<Result<Flight, FlightException>> UpdateEntity(
        Flight flight,
        string flightName,
        DateTime departureTime,
        DateTime arrivalTime,
        CancellationToken cancellationToken)
    {
        try
        {
            flight.UpdateDetails(flightName, departureTime, arrivalTime);
            return await flightRepository.Update(flight, cancellationToken);
        }
        catch (Exception exception)
        {
            return new FlightUnknownException(flight.Id, exception);
        }
    }
}