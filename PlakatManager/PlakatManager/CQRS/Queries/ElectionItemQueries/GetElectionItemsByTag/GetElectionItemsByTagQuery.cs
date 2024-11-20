using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Entities;
using MediatR;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemsByTag
{
    public class GetElectionItemsByTagQuery: IRequest<GenericResponseWithList<ElectionItem>>
    {
        public string TagName {  get; set; }
    }
}
