using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using MediatR;
using System.Text.Json.Serialization;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.GetSoonExpiringElectionItems
{
    public class GetSoonExpiringElectionItemsQuery : IRequest<GenericResponseWithList<ElectionItemDto>>
    {
        public int Days { get;set; }
        [JsonIgnore]
        public bool UserOnly { get; set; }
    }
}
