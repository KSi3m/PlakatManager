using FluentValidation;

namespace ElectionMaterialManager.CQRS.Queries.TagQueries.GetTagById
{
    public class GetTagByIdQueryValidator : AbstractValidator<GetTagByIdQuery>
    {
        public GetTagByIdQueryValidator()
        {
            RuleFor(command => command.Id).GreaterThan(0).WithMessage("Id must be larger than 1")
                .NotNull().NotEmpty();
        }
    }
}
