using FluentValidation;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.DeleteElectionItem
{
    public class DeleteElectionItemCommandValidator : AbstractValidator<DeleteElectionItemCommand>
    {
        public DeleteElectionItemCommandValidator()
        {

            RuleFor(command => command.Id).GreaterThan(0).WithMessage("Id must be larger than 1")
                .NotNull().NotEmpty();
        }
    }
}
