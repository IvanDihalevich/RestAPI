using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Passengers;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class PassengerRepository(ApplicationDbContext context) : IPassengerRepository, IPassengerQueries
{
    public async Task<Option<Passenger>> GetById(PassengerId id, CancellationToken cancellationToken)
    {
        var entity = await context.Passengers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Passenger>() : Option.Some(entity);
    }

    public async Task<IReadOnlyList<Passenger>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Passengers
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Passenger> Add(Passenger passenger, CancellationToken cancellationToken)
    {
        await context.Passengers.AddAsync(passenger, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return passenger;
    }

    public async Task<Passenger> Update(Passenger passenger, CancellationToken cancellationToken)
    {
        context.Passengers.Update(passenger);
        await context.SaveChangesAsync(cancellationToken);

        return passenger;
    }

    public async Task<Passenger> Delete(Passenger passenger, CancellationToken cancellationToken)
    {
        context.Passengers.Remove(passenger);
        await context.SaveChangesAsync(cancellationToken);

        return passenger;
    }
}