using Domain.Passengers;

namespace Api.Dtos.Passengers;

public record CreatePassengerDto(
    string FirstName,
    string LastName,
    int Age)
{
    public static CreatePassengerDto FromDomainModel(Passenger passenger)
        => new(
            FirstName: passenger.FirstName,
            LastName: passenger.LastName,
            Age: passenger.Age);
}