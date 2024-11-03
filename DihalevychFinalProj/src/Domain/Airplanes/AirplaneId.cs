namespace Domain.Airplanes;

public record AirplaneId(Guid Value)
{
    public static AirplaneId New() => new(Guid.NewGuid());
    public static AirplaneId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}