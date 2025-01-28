using Xunit;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateBillboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionMaterialManager.CQRS.Commands.AuthenticationCommands.Register;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using FluentValidation.TestHelper;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateBillboard.Tests
{
    public class CreateBillboardCommandValidatorTests
    {
        [Fact()]
        public void ValidateCreateBillboardCommandValidator_WithCorrectData_ShouldNotHaveAnyValidationErrors()
        {

            var validator = new CreateBillboardCommandValidator();
            var command = new CreateBillboardCommand()
            {
                  Location = new LocationDto() { Latitude = 22.2, Longitude = 52.2 },
                  Priority = 5, 
                  Size = "15x2m",
                  Cost = 125.5m,
                  StatusId = 1,
                  Height = 10,
                  Tags = new List<int>() { 1,2},
                  StartDate = DateTime.Parse("2025-01-28 14:30:00"),
                  EndDate = DateTime.Parse("2025-02-28 14:30:00")
                   
            };

            var result = validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
        [Fact()]
        public void ValidateCreateBillboardCommandValidator_WithInCorrectData_ShouldHaveValidationErrors()
        {

            var validator = new CreateBillboardCommandValidator();
            var command = new CreateBillboardCommand()
            {
                Location = new LocationDto() { Latitude = -91, Longitude = 189 },
                Priority = 11,
                Size = "15x200000000000000000000000000m",
                Cost = 1999999.5m,
                StatusId = 0,
                Tags = new List<int>(),
                StartDate = DateTime.Parse("2025-02-28 14:30:00"),
                EndDate = DateTime.Parse("2025-01-28 14:30:00")

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