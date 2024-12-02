using FluentValidation;

namespace ElectionMaterialManager.CQRS.Commands.TagCommands.UpdateTag
{
    public class UpdateTagCommandValidator: AbstractValidator<UpdateTagCommand>
    {

        public UpdateTagCommandValidator()
        {

            RuleFor(command => command.NewTagName).NotEmpty().NotNull().WithMessage("New tag name cannot be empty")
               .MinimumLength(1).MaximumLength(20);

        }
    }
}
