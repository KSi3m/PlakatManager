using Xunit;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.AddCommentToElectionItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionMaterialManager.CQRS.Commands.AuthenticationCommands.Register;
using FluentValidation.TestHelper;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.AddCommentToElectionItem.Tests
{
    public class AddCommentToElectionItemCommandValidatorTests
    {
        [Fact()]
        public void Validate_WithCorrectData_ShouldNotHaveValidationErrors()
        {

            var validator = new AddCommentToElectionItemCommandValidator();
            var command = new AddCommentToElectionItemCommand()
            {
                 Message = "Test"
            };

            var result = validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();

        }
        [Fact()]
        public void Validate_WithIncorrectData_ShouldHaveValidationError()
        {

            var validator = new AddCommentToElectionItemCommandValidator();
            var command = new AddCommentToElectionItemCommand()
            {
                Message = ""
            };

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Message);

        }
    }
}