using FluentValidation;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemById
{
    public class GetElectionItemByIdValidator: AbstractValidator<GetElectionItemByIdQuery>
    {
        public GetElectionItemByIdValidator()
        {

            RuleFor(command => command.Id).GreaterThan(0).WithMessage("Id must be larger than 1")
                .NotNull().NotEmpty();

        }
    }
}
