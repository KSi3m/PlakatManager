using FluentValidation;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItems
{
    public class GetElectionItemsQueryValidator: AbstractValidator<GetElectionItemsQuery>
    {
        public GetElectionItemsQueryValidator()
        {

            RuleFor(x => x.IndexRangeStart).LessThan(x => x.IndexRangeEnd)
                .WithMessage("IndexRangeStart must be lower than the IndexRangeEnd");
            RuleFor(x => x.IndexRangeEnd).GreaterThan(x => x.IndexRangeStart)
                .WithMessage("IndexRangeEnd must be larger than the IndexRangeStart"); ;


        }
    }
}
