using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using MediatR;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemsByDistrict
{
    public class GetElectionItemsByDistrictQuery :IRequest<GenericResponseWithList<ElectionItemDto>>
    {
        public string District { get; set; }
    }
}
