using Api.Dtos.Airplanes;
using FluentValidation;

namespace Api.Modules.Validators;

public class AirplaneDtoValidator : AbstractValidator<AirplaneDto>
{
    public AirplaneDtoValidator()
    {
        RuleFor(x => x.Model).NotEmpty().MaximumLength(255).MinimumLength(3);
        RuleFor(x => x.MaxPassenger).GreaterThan(0);
        RuleFor(x => x.YearOfManufacture).GreaterThan(1900).LessThanOrEqualTo(DateTime.Now.Year);
        RuleFor(x => x.AirportId).NotEmpty();
    }
}