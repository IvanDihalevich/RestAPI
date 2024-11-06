using Api.Dtos;
using Api.Dtos.Tickets;
using Api.Modules.Errors;
using Application.Tickets.Commands;
using Application.Common.Interfaces.Queries;
using Domain.Tickets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("tickets")]
[ApiController]
public class TicketsController(ISender sender, ITicketQueries ticketQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TicketDto>>> GetAll(CancellationToken cancellationToken)
    {
        var tickets = await ticketQueries.GetAll(cancellationToken);
        return tickets.Select(TicketDto.FromDomainModel).ToList();
    }

    [HttpGet("{ticketId:guid}")]
    public async Task<ActionResult<TicketDto>> Get([FromRoute] Guid ticketId, CancellationToken cancellationToken)
    {
        var ticket = await ticketQueries.GetById(new TicketId(ticketId), cancellationToken);

        return ticket.Match<ActionResult<TicketDto>>(
            t => TicketDto.FromDomainModel(t),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<CreateTicketDto>> Create([FromBody] CreateTicketDto request, CancellationToken cancellationToken)
    {
        var input = new CreateTicketCommand
        {
            FlightId = request.FlightId,
            PassengerId = request.PassengerId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CreateTicketDto>>(
            ticket => CreateTicketDto.FromDomainModel(ticket),
            e => e.ToObjectResult());
    }

    [HttpPut("{ticketId:guid}")]
    public async Task<ActionResult<UpdateTicketDto>> Update([FromRoute] Guid ticketId, [FromBody] UpdateTicketDto request, CancellationToken cancellationToken)
    {
        var input = new UpdateTicketCommand
        {
            TicketId = ticketId,
            FlightId = request.FlightId,
            PassengerId = request.PassengerId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<UpdateTicketDto>>(
            ticket => UpdateTicketDto.FromDomainModel(ticket),
            e => e.ToObjectResult());
    }

    [HttpDelete("{ticketId:guid}")]
    public async Task<ActionResult<TicketDto>> Delete([FromRoute] Guid ticketId, CancellationToken cancellationToken)
    {
        var input = new DeleteTicketCommand { TicketId = ticketId };
        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<TicketDto>>(
            ticket => TicketDto.FromDomainModel(ticket),
            e => e.ToObjectResult());
    }
}
