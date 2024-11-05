using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Passengers.Exceptions;
using Domain.Passengers;
using MediatR;

namespace Application.Passengers.Commands;

public record UpdatePassengerCommand : IRequest<Result<Passenger, PassengerException>>
{
    public required Guid PassengerId { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required int Age { get; init; }
}

public class UpdatePassengerCommandHandler : IRequestHandler<UpdatePassengerCommand, Result<Passenger, PassengerException>>
{
    private readonly IPassengerRepository _passengerRepository;

    public UpdatePassengerCommandHandler(IPassengerRepository passengerRepository)
    {
        _passengerRepository = passengerRepository;
    }

    public async Task<Result<Passenger, PassengerException>> Handle(UpdatePassengerCommand request, CancellationToken cancellationToken)
    {
        var passengerId = new PassengerId(request.PassengerId);
        var passenger = await _passengerRepository.GetById(passengerId, cancellationToken);

        return await passenger.Match(
            async p => await UpdateEntity(p, request.FirstName, request.LastName, request.Age, cancellationToken),
            () => Task.FromResult<Result<Passenger, PassengerException>>(new PassengerNotFoundException(passengerId)));
    }

    private async Task<Result<Passenger, PassengerException>> UpdateEntity(
        Passenger passenger,
        string firstName,
        string lastName,
        int age,
        CancellationToken cancellationToken)
    {
        try
        {
            passenger = Passenger.New(passenger.Id, firstName, lastName, age);
            return await _passengerRepository.Update(passenger, cancellationToken);
        }
        catch (Exception ex)
        {
            return new PassengerUnknownException(passenger.Id, ex);
        }
    }
}