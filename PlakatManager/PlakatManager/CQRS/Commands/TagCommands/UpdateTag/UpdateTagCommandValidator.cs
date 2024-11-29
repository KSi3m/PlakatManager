using FluentValidation;

namespace ElectionMaterialManager.CQRS.Commands.TagCommands.UpdateTag
{
    public class UpdateTagCommandValidator: AbstractValidator<UpdateTagCommand>
    {

        public UpdateTagCommandValidator()
        {

            RuleFor(command => command.OldTagName).NotEmpty().NotNull().WithMessage("Old tag name cannot be empty")
               .MinimumLength(1).MaximumLength(20);

            RuleFor(command => command.NewTagName).NotEmpty().NotNull().WithMessage("New tag name cannot be empty")
               .MinimumLength(1).MaximumLength(20);

        }
    }
}
