using FluentValidation;

namespace Application.Passengers.Commands;

public class DeletePassengerCommandValidator : AbstractValidator<DeletePassengerCommand>
{
    public DeletePassengerCommandValidator()
    {
        RuleFor(x => x.PassengerId).NotEmpty();
    }
}