using FluentValidation;

namespace ElectionMaterialManager.CQRS.Commands.AdminCommands.AssignAdminRole
{
    public class AssignAdminRoleCommandValidator: AbstractValidator<AssignAdminRoleCommand>
    {

        public AssignAdminRoleCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().NotNull();


        }
    }
}
