using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Tickets.Exceptions;
using Domain.Tickets;
using MediatR;

namespace Application.Tickets.Commands;

public record DeleteTicketCommand : IRequest<Result<Ticket, TicketException>>
{
    public required Guid TicketId { get; init; }
}

public class DeleteTicketCommandHandler : IRequestHandler<DeleteTicketCommand, Result<Ticket, TicketException>>
{
    private readonly ITicketRepository _ticketRepository;

    public DeleteTicketCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<Ticket, TicketException>> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
    {
        var ticketId = new TicketId(request.TicketId);
        var existingTicket = await _ticketRepository.GetById(ticketId, cancellationToken);

        return await existingTicket.Match<Task<Result<Ticket, TicketException>>>(
            async ticket => await _ticketRepository.Delete(ticket, cancellationToken),
            () => Task.FromResult<Result<Ticket, TicketException>>(new TicketNotFoundException(ticketId)));
    }
}