﻿using ElectionMaterialManager.Entities;
using FluentValidation;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.UpdateElectionItemFully
{
    public class UpdateElectionItemFullyCommandValidator: AbstractValidator<UpdateElectionItemFullyCommand>
    {
        public UpdateElectionItemFullyCommandValidator()
        {

   

            RuleFor(command => command.Location)
                .NotNull()
                .ChildRules(location =>
                {
                    location.RuleFor(l => l.Latitude)
                        .InclusiveBetween(-90, 90)
                        .WithMessage("Latitude must be between -90 and 90.");

                    location.RuleFor(l => l.Longitude)
                        .InclusiveBetween(-180, 180)
                        .WithMessage("Longitude must be between -180 and 180.");
                });


            RuleFor(command => command.Priority)
                .InclusiveBetween(1, 10).WithMessage("Priority must be between 1 and 10.");


            RuleFor(command => command.Size)
             .NotEmpty().MaximumLength(20);

            RuleFor(command => command.Cost)
                .InclusiveBetween(0, 999999.9999m);

            RuleFor(command => command.StatusId)
                .GreaterThan(0).WithMessage("StatusId must be greater than 0.");

            RuleFor(command => command.Tags)
                .NotNull().WithMessage("Tags are required.")
                .Must(tags => tags != null && tags.Any()).WithMessage("At least one tag must be specified.")
                .Must(tags => tags != null && tags.Distinct().Count() == tags.Count()).WithMessage("Tags must be distinct.")
                .Must(tags => tags != null && tags.All(tag => tag != 0)).WithMessage("Tags must not contain zero.");


            /*  RuleFor(command => command.RefreshRate).GreaterThan(24)
                  .When(x => x.Type == "Poster");*/

            RuleFor(command => command.StartDate)
                .LessThan(command => command.EndDate).WithMessage("StartDate must be earlier than EndDate.");



            RuleFor(command => command.EndDate)
                .GreaterThan(command => command.StartDate).WithMessage("EndDate must be later than StartDate.");

 


        }
    }
}
