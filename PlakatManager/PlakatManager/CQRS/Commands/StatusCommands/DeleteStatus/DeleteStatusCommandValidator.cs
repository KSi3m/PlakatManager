using FluentValidation;

namespace ElectionMaterialManager.CQRS.Commands.StatusCommands.DeleteStatus
{
    public class DeleteStatusCommandValidator: AbstractValidator<DeleteStatusCommand>
    {

        public DeleteStatusCommandValidator()
        {
            RuleFor(command => command.Id).GreaterThan(0).NotNull().NotEmpty();
        }
    }
}
