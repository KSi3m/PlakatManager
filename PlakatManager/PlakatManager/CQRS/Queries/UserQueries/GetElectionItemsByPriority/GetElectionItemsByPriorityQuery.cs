using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using MediatR;
using System.ComponentModel;

namespace ElectionMaterialManager.CQRS.Queries.UserQueries.GetElectionItemsByPriority
{
    public class GetElectionItemsByPriorityQuery : IRequest<GenericResponseWithList<ElectionItemDto>>
    {
        [DefaultValue(1)]
        public int MinPriority { get; set; }
        [DefaultValue(10)]
        public int MaxPriority { get; set; }
    }
}
