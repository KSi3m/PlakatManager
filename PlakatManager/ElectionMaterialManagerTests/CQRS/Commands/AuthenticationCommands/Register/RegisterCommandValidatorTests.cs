using Xunit;
using ElectionMaterialManager.CQRS.Commands.AuthenticationCommands.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace ElectionMaterialManager.CQRS.Commands.AuthenticationCommands.Register.Tests
{
    public class RegisterCommandValidatorTests
    {
        [Fact()]
        public void Validate_WithCorrectData_ShouldNotHaveValidationError()
        {

            var validator = new RegisterCommandValidator();
            var command = new RegisterCommand()
            {
                Username = "Test",
                Password = "JKM123@a",
                Email = "test@example.com"
            };

            var result = validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();

        }
        [Fact()]
        public void Validate_WithInorrectData_ShouldHaveValidationError()
        {

            var validator = new RegisterCommandValidator();
            var command = new RegisterCommand()
            {
                Username = "",
                Password = "JKM123@A",
                Email = "testexample.com"
            };

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x=>x.Username);
            result.ShouldHaveValidationErrorFor(x=>x.Password);
            result.ShouldHaveValidationErrorFor(x=>x.Email);

        }
    }
}