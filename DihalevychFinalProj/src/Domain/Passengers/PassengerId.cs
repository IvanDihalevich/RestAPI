namespace Domain.Passengers;

public record PassengerId(Guid Value)
{
    public static PassengerId New() => new(Guid.NewGuid());
    public static PassengerId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}