using Domain.Passengers;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IPassengerRepository
{
    Task<Passenger> Add(Passenger passenger, CancellationToken cancellationToken);
    Task<Passenger> Update(Passenger passenger, CancellationToken cancellationToken);
    Task<Passenger> Delete(Passenger passenger, CancellationToken cancellationToken);
    Task<Option<Passenger>> GetById(PassengerId id, CancellationToken cancellationToken);
}