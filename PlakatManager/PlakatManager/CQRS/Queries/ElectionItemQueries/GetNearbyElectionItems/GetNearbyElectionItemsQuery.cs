using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using MediatR;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetNearbyElectionItems
{
    public class GetNearbyElectionItemsQuery: IRequest<GenericResponseWithList<ElectionItemDto>>
    {

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double RadiusInKm { get; set; }
    }
}
