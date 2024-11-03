using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Tickets;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class TicketRepository(ApplicationDbContext context) : ITicketRepository, ITicketQueries
{
    public async Task<Option<Ticket>> GetById(TicketId id, CancellationToken cancellationToken)
    {
        var entity = await context.Tickets
            .Include(t => t.Flight)
            .Include(t => t.Passenger)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Ticket>() : Option.Some(entity);
    }

    public async Task<IReadOnlyList<Ticket>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Tickets
            .Include(t => t.Flight)
            .Include(t => t.Passenger)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Ticket> Add(Ticket ticket, CancellationToken cancellationToken)
    {
        await context.Tickets.AddAsync(ticket, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return ticket;
    }

    public async Task<Ticket> Update(Ticket ticket, CancellationToken cancellationToken)
    {
        context.Tickets.Update(ticket);
        await context.SaveChangesAsync(cancellationToken);

        return ticket;
    }

    public async Task<Ticket> Delete(Ticket ticket, CancellationToken cancellationToken)
    {
        context.Tickets.Remove(ticket);
        await context.SaveChangesAsync(cancellationToken);

        return ticket;
    }
}