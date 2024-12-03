using ElectionMaterialManager.CQRS.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace ElectionMaterialManager.CQRS.Commands.StatusCommands.DeleteStatus
{
    public class DeleteStatusCommand: IRequest<Response>
    {
        [JsonIgnore]
        public int Id { get; set; }
    }
}
