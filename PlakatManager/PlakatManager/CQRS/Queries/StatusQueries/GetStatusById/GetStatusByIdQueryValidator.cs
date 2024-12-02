using ElectionMaterialManager.CQRS.Queries.StatusQueries.GetAllStatuses;
using FluentValidation;

namespace ElectionMaterialManager.CQRS.Queries.StatusQueries.GetStatusById
{
    public class GetStatusByIdQueryValidator : AbstractValidator<GetStatusByIdQuery>
    {

        public GetStatusByIdQueryValidator()
        {
            RuleFor(command => command.Id).GreaterThan(0).WithMessage("Id must be larger than 1")
                .NotNull().NotEmpty();

        }
    }
}
