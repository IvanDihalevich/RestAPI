using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Airplanes;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class AirplaneRepository(ApplicationDbContext context) : IAirplaneRepository, IAirplaneQueries
{
    public async Task<Option<Airplane>> GetById(AirplaneId id, CancellationToken cancellationToken)
    {
        var entity = await context.Airplanes
            .Include(a => a.Airport)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Airplane>() : Option.Some(entity);
    }

    public async Task<IReadOnlyList<Airplane>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Airplanes
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Airplane> Add(Airplane airplane, CancellationToken cancellationToken)
    {
        await context.Airplanes.AddAsync(airplane, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return airplane;
    }

    public async Task<Airplane> Update(Airplane airplane, CancellationToken cancellationToken)
    {
        context.Airplanes.Update(airplane);
        await context.SaveChangesAsync(cancellationToken);

        return airplane;
    }

    public async Task<Airplane> Delete(Airplane airplane, CancellationToken cancellationToken)
    {
        context.Airplanes.Remove(airplane);
        await context.SaveChangesAsync(cancellationToken);

        return airplane;
    }
}