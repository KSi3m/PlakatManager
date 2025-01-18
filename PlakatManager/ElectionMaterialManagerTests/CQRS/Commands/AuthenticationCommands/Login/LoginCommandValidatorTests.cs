using Xunit;
using ElectionMaterialManager.CQRS.Commands.AuthenticationCommands.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;

namespace ElectionMaterialManager.CQRS.Commands.AuthenticationCommands.Login.Tests
{
    public class LoginCommandValidatorTests
    {
        [Fact()]
        public void ValidateLogin_WithCorrectData_ShouldNotHaveValidationError()
        {
            var validator = new LoginCommandValidator();
            var command = new LoginCommand()
            {
                Login = "Test",
                Password = "JKM123@a"
            };

            var result = validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact()]
        public void ValidateLogin_WithIncorrectData_ShouldHaveValidationError()
        {
            var validator = new LoginCommandValidator();
            var command = new LoginCommand()
            {
                Login = "",
                Password = "aaaa"
            };

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Login);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }
    }
}