using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using System.ComponentModel;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItems
{
    public class GetElectionItemsQuery : IRequest<GenericResponseWithList<ElectionItemDto>>
    {
      
        public int? IndexRangeStart {  get; set; }
        public int? IndexRangeEnd { get; set; }

    }
}
