using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Airports;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class AirportRepository(ApplicationDbContext context) : IAirportRepository, IAirportQueries
{
    public async Task<Option<Airport>> GetById(AirportId id, CancellationToken cancellationToken)
    {
        var entity = await context.Airports
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Airport>() : Option.Some(entity);
    }

    public async Task<IReadOnlyList<Airport>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Airports
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Airport> Add(Airport airport, CancellationToken cancellationToken)
    {
        await context.Airports.AddAsync(airport, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return airport;
    }

    public async Task<Airport> Update(Airport airport, CancellationToken cancellationToken)
    {
        context.Airports.Update(airport);
        await context.SaveChangesAsync(cancellationToken);

        return airport;
    }

    public async Task<Airport> Delete(Airport airport, CancellationToken cancellationToken)
    {
        context.Airports.Remove(airport);
        await context.SaveChangesAsync(cancellationToken);

        return airport;
    }
}