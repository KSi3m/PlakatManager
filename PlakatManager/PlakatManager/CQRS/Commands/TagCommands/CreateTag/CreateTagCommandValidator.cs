using FluentValidation;

namespace ElectionMaterialManager.CQRS.Commands.TagCommands.CreateTag
{
    public class CreateTagCommandValidator: AbstractValidator<CreateTagCommand>
    {

        public CreateTagCommandValidator()
        {

            RuleFor(command => command.TagName).NotEmpty().NotNull().WithMessage("Tag name cannot be empty")
                .MinimumLength(1).MaximumLength(20);

        }
    }
}
