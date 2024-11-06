using Api.Dtos;
using Api.Dtos.Airports;
using Api.Modules.Errors;
using Application.Airports.Commands;
using Application.Common.Interfaces.Queries;
using Domain.Airports;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("airports")]
[ApiController]
public class AirportsController(ISender sender, IAirportQueries airportQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AirportDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await airportQueries.GetAll(cancellationToken);
        return entities.Select(AirportDto.FromDomainModel).ToList();
    }

    [HttpGet("{airportId:guid}")]
    public async Task<ActionResult<AirportDto>> Get([FromRoute] Guid airportId, CancellationToken cancellationToken)
    {
        var entity = await airportQueries.GetById(new AirportId(airportId), cancellationToken);

        return entity.Match<ActionResult<AirportDto>>(
            airport => AirportDto.FromDomainModel(airport),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<CreateAirportDto>> Create([FromBody] CreateAirportDto request, CancellationToken cancellationToken)
    {
        var input = new CreateAirportCommand
        {
            Name = request.Name,
            Location = request.Location
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CreateAirportDto>>(
            airport => CreateAirportDto.FromDomainModel(airport),
            e => e.ToObjectResult());
    }

    [HttpPut("{airportId:guid}")]
    public async Task<ActionResult<AirportDto>> Update([FromRoute] Guid airportId, [FromBody] AirportDto request, CancellationToken cancellationToken)
    {
        var input = new UpdateAirportCommand
        {
            AirportId = airportId,
            Name = request.Name,
            Location = request.Location
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<AirportDto>>(
            airport => AirportDto.FromDomainModel(airport),
            e => e.ToObjectResult());
    }

    [HttpDelete("{airportId:guid}")]
    public async Task<ActionResult<AirportDto>> Delete([FromRoute] Guid airportId, CancellationToken cancellationToken)
    {
        var input = new DeleteAirportCommand { AirportId = airportId };
        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<AirportDto>>(
            airport => AirportDto.FromDomainModel(airport),
            e => e.ToObjectResult());
    }
}
