using Api.Dtos;
using Api.Dtos.Airplanes;
using Api.Modules.Errors;
using Application.Airplanes.Commands;
using Application.Common.Interfaces.Queries;
using Domain.Airplanes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("airplanes")]
[ApiController]
public class AirplanesController(ISender sender, IAirplaneQueries airplaneQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AirplaneDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await airplaneQueries.GetAll(cancellationToken);
        return entities.Select(AirplaneDto.FromDomainModel).ToList();
    }

    [HttpGet("{airplaneId:guid}")]
    public async Task<ActionResult<AirplaneDto>> Get([FromRoute] Guid airplaneId, CancellationToken cancellationToken)
    {
        var entity = await airplaneQueries.GetById(new AirplaneId(airplaneId), cancellationToken);

        return entity.Match<ActionResult<AirplaneDto>>(
            airplane => AirplaneDto.FromDomainModel(airplane),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<CreateAirplaneDto>> Create([FromBody] CreateAirplaneDto request, CancellationToken cancellationToken)
    {
        var input = new CreateAirplaneCommand
        {
            Model = request.Model,
            MaxPassenger = request.MaxPassenger,
            YearOfManufacture = request.YearOfManufacture,
            AirportId = request.AirportId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CreateAirplaneDto>>(
            airplane => CreateAirplaneDto.FromDomainModel(airplane),
            e => e.ToObjectResult());
    }

    [HttpPut]
    public async Task<ActionResult<UpdateAirplaneDto>> Update([FromBody] UpdateAirplaneDto request, CancellationToken cancellationToken)
    {
        var input = new UpdateAirplaneCommand
        {
            AirplaneId = request.AirplaneId,
            Model = request.Model,
            MaxPassenger = request.MaxPassenger,
            YearOfManufacture = request.YearOfManufacture
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<UpdateAirplaneDto>>(
            airplane => UpdateAirplaneDto.FromDomainModel(airplane),
            e => e.ToObjectResult());
    }

    [HttpDelete("{airplaneId:guid}")]
    public async Task<ActionResult<AirplaneDto>> Delete([FromRoute] Guid airplaneId, CancellationToken cancellationToken)
    {
        var input = new DeleteAirplaneCommand { AirplaneId = airplaneId };
        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<AirplaneDto>>(
            airplane => AirplaneDto.FromDomainModel(airplane),
            e => e.ToObjectResult());
    }
}
