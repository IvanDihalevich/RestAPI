using FluentValidation;

namespace Application.Airplanes.Commands;

public class UpdateAirplaneCommandValidator : AbstractValidator<UpdateAirplaneCommand>
{
    public UpdateAirplaneCommandValidator()
    {
        RuleFor(x => x.Model).NotEmpty().MaximumLength(255).MinimumLength(3);
        RuleFor(x => x.MaxPassenger).GreaterThan(0);
        RuleFor(x => x.YearOfManufacture).GreaterThan(1900).LessThanOrEqualTo(DateTime.Now.Year);
        RuleFor(x => x.AirplaneId).NotEmpty();
    }
}