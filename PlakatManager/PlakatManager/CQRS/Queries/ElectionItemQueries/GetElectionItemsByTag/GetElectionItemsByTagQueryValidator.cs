using FluentValidation;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemsByTag
{
    public class GetElectionItemsByTagQueryValidator : AbstractValidator<GetElectionItemsByTagQuery>
    {

        public GetElectionItemsByTagQueryValidator()
        {

            RuleFor(x => x.TagName).NotEmpty().NotNull();
        }
    }
}
