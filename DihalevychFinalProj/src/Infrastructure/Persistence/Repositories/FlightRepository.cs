using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Flights;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class FlightRepository(ApplicationDbContext context) : IFlightRepository, IFlightQueries
{
    public async Task<Option<Flight>> GetById(FlightId id, CancellationToken cancellationToken)
    {
        var entity = await context.Flights
            .Include(f => f.Airplane)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Flight>() : Option.Some(entity);
    }

    public async Task<IReadOnlyList<Flight>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Flights
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Flight> Add(Flight flight, CancellationToken cancellationToken)
    {
        await context.Flights.AddAsync(flight, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return flight;
    }

    public async Task<Flight> Update(Flight flight, CancellationToken cancellationToken)
    {
        context.Flights.Update(flight);
        await context.SaveChangesAsync(cancellationToken);

        return flight;
    }

    public async Task<Flight> Delete(Flight flight, CancellationToken cancellationToken)
    {
        context.Flights.Remove(flight);
        await context.SaveChangesAsync(cancellationToken);

        return flight;
    }
}