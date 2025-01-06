using FluentValidation;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetNearbyElectionItems
{
    public class GetNearbyElectionItemsQueryValidator : AbstractValidator<GetNearbyElectionItemsQuery>
    {
        public GetNearbyElectionItemsQueryValidator()
        {
            RuleFor(query => query.Latitude)
              .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");

            RuleFor(query => query.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");

            RuleFor(query => query.RadiusInKm).GreaterThan(0.0);

        }
    }
}
