using FluentValidation;

namespace ElectionMaterialManager.CQRS.Commands.StatusCommands.CreateStatus
{
    public class CreateStatusCommandValidator : AbstractValidator<CreateStatusCommand>
    {
        public CreateStatusCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotEmpty().NotNull()
                .WithMessage("Status name cannot be empty")
                .MinimumLength(1).MaximumLength(20);

        }
    }
}
