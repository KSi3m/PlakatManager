using FluentValidation;

namespace ElectionMaterialManager.CQRS.Commands.TagCommands.DeleteTag
{
    public class DeleteTagCommandValidator: AbstractValidator<DeleteTagCommand>
    {
        public DeleteTagCommandValidator()
        {
            RuleFor(command => command.Id).GreaterThan(0).WithMessage("Id must be larger than 1")
                .NotNull().NotEmpty();
        }
    }
}
