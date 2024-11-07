using Domain.Passengers;

namespace Tests.Data;

public static class PassengersData
{
    public static Passenger MainPassenger() 
        => Passenger.New(PassengerId.New(), "John", "Doe", 30);
}