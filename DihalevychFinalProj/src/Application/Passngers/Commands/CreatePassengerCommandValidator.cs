using FluentValidation;

namespace Application.Passengers.Commands;

public class CreatePassengerCommandValidator : AbstractValidator<CreatePassengerCommand>
{
    public CreatePassengerCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Age).GreaterThan(0);
    }
}