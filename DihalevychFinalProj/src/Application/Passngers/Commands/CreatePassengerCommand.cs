using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Passengers.Exceptions;
using Domain.Passengers;
using MediatR;

namespace Application.Passengers.Commands;

public record CreatePassengerCommand : IRequest<Result<Passenger, PassengerException>>
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required int Age { get; init; }
}

public class CreatePassengerCommandHandler : IRequestHandler<CreatePassengerCommand, Result<Passenger, PassengerException>>
{
    private readonly IPassengerRepository _passengerRepository;

    public CreatePassengerCommandHandler(IPassengerRepository passengerRepository)
    {
        _passengerRepository = passengerRepository;
    }

    public async Task<Result<Passenger, PassengerException>> Handle(
        CreatePassengerCommand request,
        CancellationToken cancellationToken)
    {
        var passenger = Passenger.New(PassengerId.New(), request.FirstName, request.LastName, request.Age);
        
        try
        {
            return await _passengerRepository.Add(passenger, cancellationToken);
        }
        catch (Exception ex)
        {
            return new PassengerUnknownException(passenger.Id, ex);
        }
    }
}