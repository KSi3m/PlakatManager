using FluentValidation;

namespace ElectionMaterialManager.CQRS.Commands.StatusCommands.UpdateStatus
{
    public class UpdateStatusCommandValidator: AbstractValidator<UpdateStatusCommand>
    {

        public UpdateStatusCommandValidator()
        {


            RuleFor(command => command.NewStatusName)
               .NotEmpty().NotNull().WithMessage("New status name cannot be empty")
               .MinimumLength(1).MaximumLength(20);

        }
    }
}
