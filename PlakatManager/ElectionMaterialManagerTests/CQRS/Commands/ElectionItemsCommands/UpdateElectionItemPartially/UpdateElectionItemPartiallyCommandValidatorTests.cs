using Xunit;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.UpdateElectionItemPartially;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.UpdateElectionItemFully;
using ElectionMaterialManager.Dtos;
using FluentValidation.TestHelper;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.UpdateElectionItemPartially.Tests
{
    public class UpdateElectionItemPartiallyCommandValidatorTests
    {
        [Fact()]
        public void ValidateUpdateElectionItemPartiallyCommandValidator_WithCorrectData_ShouldNotHaveAnyValidationErrors()
        {

            var validator = new UpdateElectionItemPartiallyCommandValidator();
            var command = new UpdateElectionItemPartiallyCommand()
            {
                Location = new LocationDto() { Latitude = 25.2, Longitude = 58.2 },
                Priority = 2,
                Size = "1x2m",
                Cost = 29.6m,
                StatusId = 1,
                RefreshRate = 32,
                Tags = new List<int>() { 1, 2 },
                StartDate = DateTime.Parse("2025-01-28 14:30:00"),
                EndDate = DateTime.Parse("2025-02-28 14:30:00")

            };

            var result = validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
        [Fact()]
        public void ValidateUpdateElectionItemPartiallyCommandValidator_WithInCorrectData_ShouldHaveValidationErrors()
        {

            var validator = new UpdateElectionItemPartiallyCommandValidator();
            var command = new UpdateElectionItemPartiallyCommand()
            {
                Priority = 15,
                Size = "134234x200000000000000000000000000m",
                Cost = 2999999.5m,
                StatusId = -1,
                RefreshRate = 22,
                Tags = new List<int>() { 0 },
                StartDate = DateTime.Parse("2025-02-28 14:30:00"),
                EndDate = DateTime.Parse("2025-01-28 14:30:00")

            };

            var result = validator.TestValidate(command);

           


            result.ShouldHaveValidationErrorFor(x => x.Priority);
            result.ShouldHaveValidationErrorFor(x => x.Size);
            result.ShouldHaveValidationErrorFor(x => x.Cost);
            result.ShouldHaveValidationErrorFor(x => x.StatusId);

            result.ShouldHaveValidationErrorFor(x => x.StartDate);
            result.ShouldHaveValidationErrorFor(x => x.EndDate);

        }
    }
}