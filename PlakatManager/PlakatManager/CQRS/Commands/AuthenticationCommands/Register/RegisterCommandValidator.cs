using FluentValidation;

namespace ElectionMaterialManager.CQRS.Commands.AuthenticationCommands.Register
{
    public class RegisterCommandValidator: AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator() {


            RuleFor(x => x.Username)
              .NotEmpty().WithMessage("Username is required.");


            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches("[!\"#$%&'()*+,./:;<=>?@[\\]^_`{|}~]").WithMessage("Password must contain at least one special character.");



            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(50);

        }

    }
}
