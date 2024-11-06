using Api.Dtos.Airports;
using FluentValidation;

namespace Api.Modules.Validators;

public class AirportDtoValidator : AbstractValidator<AirportDto>
{
    public AirportDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255).MinimumLength(3);
        RuleFor(x => x.Location).NotEmpty().MaximumLength(255).MinimumLength(3);
    }
}
