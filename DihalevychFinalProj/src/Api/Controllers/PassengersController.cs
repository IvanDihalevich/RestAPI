using Api.Dtos;
using Api.Dtos.Passengers;
using Api.Modules.Errors;
using Application.Passengers.Commands;
using Application.Common.Interfaces.Queries;
using Domain.Passengers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("passengers")]
[ApiController]
public class PassengersController(ISender sender, IPassengerQueries passengerQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PassengerDto>>> GetAll(CancellationToken cancellationToken)
    {
        var passengers = await passengerQueries.GetAll(cancellationToken);
        return passengers.Select(PassengerDto.FromDomainModel).ToList();
    }

    [HttpGet("{passengerId:guid}")]
    public async Task<ActionResult<PassengerDto>> Get([FromRoute] Guid passengerId, CancellationToken cancellationToken)
    {
        var passenger = await passengerQueries.GetById(new PassengerId(passengerId), cancellationToken);

        return passenger.Match<ActionResult<PassengerDto>>(
            p => PassengerDto.FromDomainModel(p),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<CreatePassengerDto>> Create([FromBody] CreatePassengerDto request, CancellationToken cancellationToken)
    {
        var input = new CreatePassengerCommand
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Age = request.Age
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CreatePassengerDto>>(
            passenger => CreatePassengerDto.FromDomainModel(passenger),
            e => e.ToObjectResult());
    }

    [HttpPut("{passengerId:guid}")]
    public async Task<ActionResult<PassengerDto>> Update([FromRoute] Guid passengerId, [FromBody] PassengerDto request, CancellationToken cancellationToken)
    {
        var input = new UpdatePassengerCommand
        {
            PassengerId = passengerId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Age = request.Age
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<PassengerDto>>(
            passenger => PassengerDto.FromDomainModel(passenger),
            e => e.ToObjectResult());
    }

    [HttpDelete("{passengerId:guid}")]
    public async Task<ActionResult<PassengerDto>> Delete([FromRoute] Guid passengerId, CancellationToken cancellationToken)
    {
        var input = new DeletePassengerCommand { PassengerId = passengerId };
        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<PassengerDto>>(
            passenger => PassengerDto.FromDomainModel(passenger),
            e => e.ToObjectResult());
    }
}
