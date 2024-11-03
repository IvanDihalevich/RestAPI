using Domain.Tickets;

namespace Domain.Passengers;

public class Passenger
{
    public PassengerId Id { get; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public int Age { get; private set; }

    private Passenger(PassengerId id, string firstName, string lastName, int age)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Age = age;
    }

    public static Passenger New(PassengerId id, string firstName, string lastName, int age)
        => new(id, firstName, lastName, age);
    
}