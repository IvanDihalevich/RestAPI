using Api.Dtos;
using Api.Dtos.Flights;
using Api.Modules.Errors;
using Application.Flights.Commands;
using Application.Common.Interfaces.Queries;
using Domain.Flights;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("flights")]
[ApiController]
public class FlightsController(ISender sender, IFlightQueries flightQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FlightDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await flightQueries.GetAll(cancellationToken);
        return entities.Select(FlightDto.FromDomainModel).ToList();
    }

    [HttpGet("{flightId:guid}")]
    public async Task<ActionResult<FlightDto>> Get([FromRoute] Guid flightId, CancellationToken cancellationToken)
    {
        var entity = await flightQueries.GetById(new FlightId(flightId), cancellationToken);

        return entity.Match<ActionResult<FlightDto>>(
            flight => FlightDto.FromDomainModel(flight),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<CreateFlightDto>> Create([FromBody] CreateFlightDto request, CancellationToken cancellationToken)
    {
        var input = new CreateFlightCommand
        {
            FlightName = request.FlightName,
            DepartureTime = request.DepartureTime,
            ArrivalTime = request.ArrivalTime,
            DepartureAirportId = request.DepartureAirportId,
            ArrivalAirportId = request.ArrivalAirportId,
            AirplaneId = request.AirplaneId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CreateFlightDto>>(
            flight => CreateFlightDto.FromDomainModel(flight),
            e => e.ToObjectResult());
    }

    [HttpPut("{flightId:guid}")]
    public async Task<ActionResult<UpdateFlightDto>> Update([FromRoute] Guid flightId, [FromBody] UpdateFlightDto request, CancellationToken cancellationToken)
    {
        var input = new UpdateFlightCommand
        {
            FlightId = flightId,
            FlightName = request.FlightName,
            DepartureTime = request.DepartureTime,
            ArrivalTime = request.ArrivalTime
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<UpdateFlightDto>>(
            flight => UpdateFlightDto.FromDomainModel(flight),
            e => e.ToObjectResult());
    }

    [HttpDelete("{flightId:guid}")]
    public async Task<ActionResult<FlightDto>> Delete([FromRoute] Guid flightId, CancellationToken cancellationToken)
    {
        var input = new DeleteFlightCommand { FlightId = flightId };
        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<FlightDto>>(
            flight => FlightDto.FromDomainModel(flight),
            e => e.ToObjectResult());
    }
}
