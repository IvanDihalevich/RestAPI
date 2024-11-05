using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Tickets.Exceptions;
using Domain.Flights;
using Domain.Passengers;
using Domain.Tickets;
using MediatR;

namespace Application.Tickets.Commands;

public record UpdateTicketCommand : IRequest<Result<Ticket, TicketException>>
{
    public required Guid TicketId { get; init; }
    public required Guid FlightId { get; init; }
    public required Guid PassengerId { get; init; }
}

public class UpdateTicketCommandHandler(ITicketRepository ticketRepository)
    : IRequestHandler<UpdateTicketCommand, Result<Ticket, TicketException>>
{
    public async Task<Result<Ticket, TicketException>> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticketId = new TicketId(request.TicketId);
        var existingTicket = await ticketRepository.GetById(ticketId, cancellationToken);

        return await existingTicket.Match<Task<Result<Ticket, TicketException>>>(
            async t =>
            {
                var updatedTicket = Ticket.New(ticketId, new FlightId(request.FlightId), new PassengerId(request.PassengerId));
                return await ticketRepository.Update(updatedTicket, cancellationToken);
            },
            () => Task.FromResult<Result<Ticket, TicketException>>(new TicketNotFoundException(ticketId))
        );
    }
    //ToDo:
}