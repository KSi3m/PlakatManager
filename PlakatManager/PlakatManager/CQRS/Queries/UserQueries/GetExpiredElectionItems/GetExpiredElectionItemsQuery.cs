using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using MediatR;
using System.Text.Json.Serialization;

namespace ElectionMaterialManager.CQRS.Queries.UserQueries.GetExpiredElectionItems
{
    public class GetExpiredElectionItemsQuery : IRequest<GenericResponseWithList<ElectionItemDto>>
    {
        [JsonIgnore]
        public bool UserOnly { get; set; }
    }
}
