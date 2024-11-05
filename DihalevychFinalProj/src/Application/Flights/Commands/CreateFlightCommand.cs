using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Flights.Exceptions;
using Domain.Airplanes;
using Domain.Airports;
using Domain.Flights;
using MediatR;

namespace Application.Flights.Commands;

public record CreateFlightCommand : IRequest<Result<Flight, FlightException>>
{
    public required string FlightName { get; init; }
    public required DateTime DepartureTime { get; init; }
    public required DateTime ArrivalTime { get; init; }
    public required Guid DepartureAirportId { get; init; }
    public required Guid ArrivalAirportId { get; init; }
    public required Guid AirplaneId { get; init; }
}

public class CreateFlightCommandHandler(
    IFlightRepository flightRepository)
    : IRequestHandler<CreateFlightCommand, Result<Flight, FlightException>>
{
    public async Task<Result<Flight, FlightException>> Handle(CreateFlightCommand request, CancellationToken cancellationToken)
    {
        var flight = Flight.New(
            FlightId.New(),
            request.FlightName,
            request.DepartureTime,
            request.ArrivalTime,
            new AirportId(request.DepartureAirportId),
            new AirportId(request.ArrivalAirportId),
            new AirplaneId(request.AirplaneId));

        return await flightRepository.Add(flight, cancellationToken);
    }
}