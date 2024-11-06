using Api.Dtos.Passengers;
using FluentValidation;

namespace Api.Modules.Validators;

public class PassengerDtoValidator : AbstractValidator<PassengerDto>
{
    public PassengerDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Age).GreaterThan(0);
    }
}
