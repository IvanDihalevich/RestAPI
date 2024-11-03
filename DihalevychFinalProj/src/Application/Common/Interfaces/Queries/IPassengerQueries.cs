using Domain.Passengers;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IPassengerQueries
{
    Task<IReadOnlyList<Passenger>> GetAll(CancellationToken cancellationToken);
    Task<Option<Passenger>> GetById(PassengerId id, CancellationToken cancellationToken);
}