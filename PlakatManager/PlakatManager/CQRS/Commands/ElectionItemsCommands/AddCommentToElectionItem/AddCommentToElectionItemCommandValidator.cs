using FluentValidation;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.AddCommentToElectionItem
{
    public class AddCommentToElectionItemCommandValidator : AbstractValidator<AddCommentToElectionItemCommand>
    {
        public AddCommentToElectionItemCommandValidator()
        {
            RuleFor(command => command.Message)
                .NotEmpty().NotNull().WithMessage("Message cannot be empty");



        }
    }
}
