using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Passengers.Exceptions;
using Domain.Passengers;
using MediatR;

namespace Application.Passengers.Commands;

public record DeletePassengerCommand : IRequest<Result<Passenger, PassengerException>>
{
    public required Guid PassengerId { get; init; }
}

public class DeletePassengerCommandHandler(IPassengerRepository passengerRepository)
    : IRequestHandler<DeletePassengerCommand, Result<Passenger, PassengerException>>
{
    public async Task<Result<Passenger, PassengerException>> Handle(
        DeletePassengerCommand request,
        CancellationToken cancellationToken)
    {
        var passengerId = new PassengerId(request.PassengerId);

        var existingPassenger = await passengerRepository.GetById(passengerId, cancellationToken);

        return await existingPassenger.Match<Task<Result<Passenger, PassengerException>>>(
            async p => await DeleteEntity(p, cancellationToken),
            () => Task.FromResult<Result<Passenger, PassengerException>>(new PassengerNotFoundException(passengerId)));
    }

    public async Task<Result<Passenger, PassengerException>> DeleteEntity(Passenger passenger, CancellationToken cancellationToken)
    {
        try
        {
            return await passengerRepository.Delete(passenger, cancellationToken);
        }
        catch (Exception exception)
        {
            return new PassengerUnknownException(passenger.Id, exception);
        }
    }
}