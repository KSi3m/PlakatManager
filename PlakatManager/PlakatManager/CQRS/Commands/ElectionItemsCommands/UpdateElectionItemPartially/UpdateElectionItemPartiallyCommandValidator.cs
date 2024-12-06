using FluentValidation;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.UpdateElectionItemPartially
{
    public class UpdateElectionItemPartiallyCommandValidator : AbstractValidator<UpdateElectionItemPartiallyCommand>
    {
        public UpdateElectionItemPartiallyCommandValidator()
        {




            RuleFor(command => command.Area)
                 .MaximumLength(200);


            RuleFor(command => command.Location.Latitude2)
                .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");

            RuleFor(command => command.Location.Longitude2)
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");


            RuleFor(command => command.Priority)
                .InclusiveBetween(1, 10).WithMessage("Priority must be between 1 and 10.")
                .When(command => command.Priority.HasValue);



            RuleFor(command => command.Size)
               .MaximumLength(20);

            RuleFor(command => command.Cost)
                .InclusiveBetween(0, 999999.9999m);

            RuleFor(command => command.StatusId)
                .GreaterThan(0).WithMessage("StatusId must be greater than 0.");

            RuleFor(command => command.Tags)
                //.NotNull().WithMessage("Tags are required.")
                .Must(tags => tags == null || tags.Any())
                .WithMessage("If specified, at least one tag must be provided");


            RuleFor(command => command.StartDate)
                .LessThan(command => command.EndDate).WithMessage("StartDate must be earlier than EndDate.")
                .When(command => command.StartDate.HasValue && command.EndDate.HasValue);

            RuleFor(command => command.EndDate)
                .GreaterThan(command => command.StartDate).WithMessage("EndDate must be later than StartDate.")
                .When(command => command.StartDate.HasValue && command.EndDate.HasValue);
            

        }
    }
}
