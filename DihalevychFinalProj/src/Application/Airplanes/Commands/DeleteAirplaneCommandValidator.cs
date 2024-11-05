using FluentValidation;

namespace Application.Airplanes.Commands;

public class DeleteAirplaneCommandValidator : AbstractValidator<DeleteAirplaneCommand>
{
    public DeleteAirplaneCommandValidator()
    {
        RuleFor(x => x.AirplaneId).NotEmpty();
    }
}