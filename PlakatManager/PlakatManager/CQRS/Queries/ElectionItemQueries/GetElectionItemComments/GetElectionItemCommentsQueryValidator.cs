using FluentValidation;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemComments
{
    public class GetElectionItemCommentsQueryValidator: AbstractValidator<GetElectionItemCommentsQuery>
    {

        public GetElectionItemCommentsQueryValidator()
        {

            RuleFor(command => command.Id).GreaterThan(0).WithMessage("Id must be larger than 1")
                .NotNull().NotEmpty();

        }
    }
}
