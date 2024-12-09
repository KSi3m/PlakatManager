using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using System.ComponentModel;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItems
{
    public class GetElectionItemsQuery : IRequest<GenericResponseWithList<ElectionItemDto>>
    {
        [DefaultValue(1)]
        public int? IndexRangeStart {  get; set; }
        [DefaultValue(10)]
        public int? IndexRangeEnd { get; set; }

    }
}
