using FluentValidation;

namespace Application.Passengers.Commands;

public class UpdatePassengerCommandValidator : AbstractValidator<UpdatePassengerCommand>
{
    public UpdatePassengerCommandValidator()
    {
        RuleFor(x => x.PassengerId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Age).GreaterThan(0);
    }
}