using Xunit;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreatePoster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateLED;
using ElectionMaterialManager.Dtos;
using FluentValidation.TestHelper;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreatePoster.Tests
{
    public class CreatePosterCommandValidatorTests
    {
        [Fact()]
        public void ValidateCreatePosterCommandValidator_WithCorrectData_ShouldNotHaveAnyValidationErrors()
        {

            var validator = new CreatePosterCommandValidator();
            var command = new CreatePosterCommand()
            {
                Location = new LocationDto() { Latitude = 22.2, Longitude = 52.2 },
                Priority = 6,
                Size = "15x8m",
                Cost = 12.5m,
                StatusId = 2,
                Tags = new List<int>() { 4, 2 },
                StartDate = DateTime.Parse("2025-01-28 14:30:00"),
                EndDate = DateTime.Parse("2025-03-28 14:30:00")

            };

            var result = validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
        [Fact()]
        public void ValidateCreatePosterCommandValidator_WithInCorrectData_ShouldHaveValidationErrors()
        {

            var validator = new CreatePosterCommandValidator();
            var command = new CreatePosterCommand()
            {
                Location = new LocationDto() { Latitude = -95, Longitude = 195 },
                Priority = 12,
                Size = "15x200000000000000000000000000m",
                Cost = 2359999.5m,
                StatusId = 0,
                Tags = new List<int>() { 1,1 },
                StartDate = DateTime.Parse("2025-02-28 14:30:00"),
                EndDate = DateTime.Parse("2025-01-20 12:20:00")

            };

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Location.Longitude);
            result.ShouldHaveValidationErrorFor(x => x.Location.Latitude);
            result.ShouldHaveValidationErrorFor(x => x.Priority);
            result.ShouldHaveValidationErrorFor(x => x.Size);
            result.ShouldHaveValidationErrorFor(x => x.Cost);
            result.ShouldHaveValidationErrorFor(x => x.StatusId);
            result.ShouldHaveValidationErrorFor(x => x.Tags);
            result.ShouldHaveValidationErrorFor(x => x.StartDate);
            result.ShouldHaveValidationErrorFor(x => x.EndDate);

        }
    }
}