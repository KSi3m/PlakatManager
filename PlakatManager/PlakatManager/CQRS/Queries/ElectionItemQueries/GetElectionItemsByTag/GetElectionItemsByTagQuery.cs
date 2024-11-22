using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemsByTag
{
    public class GetElectionItemsByTagQuery: IRequest<GenericResponseWithList<ElectionItemDto>>
    {
        public string TagName {  get; set; }
    }
}
