using FluentValidation;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemsByDistrict
{
    public class GetElectionItemsByDistrictQueryValidator : AbstractValidator<GetElectionItemsByDistrictQuery>
    {
        public GetElectionItemsByDistrictQueryValidator()
        {

            RuleFor(x => x.District).NotEmpty().NotNull();
        }
    }
}
