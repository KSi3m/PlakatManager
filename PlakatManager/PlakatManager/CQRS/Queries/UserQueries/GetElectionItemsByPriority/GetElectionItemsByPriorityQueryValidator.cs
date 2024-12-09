using FluentValidation;

namespace ElectionMaterialManager.CQRS.Queries.UserQueries.GetElectionItemsByPriority
{
    public class GetElectionItemsByPriorityQueryValidator : AbstractValidator<GetElectionItemsByPriorityQuery>
    {
        public GetElectionItemsByPriorityQueryValidator()
        {

            RuleFor(x => x.MinPriority).LessThanOrEqualTo(x => x.MaxPriority)
            .WithMessage("MinPriority must be lower than the MaxPriority")
            .GreaterThan(0).LessThan(11);
            RuleFor(x => x.MaxPriority).GreaterThanOrEqualTo(x => x.MinPriority)
                .WithMessage("MaxPriority must be larger than the MinPriority")
            .GreaterThan(0).LessThan(11); ;

        }
    }
}
