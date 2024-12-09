using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using MediatR;
using System.Text.Json.Serialization;

namespace ElectionMaterialManager.CQRS.Commands.StatusCommands.UpdateStatus
{
    public class UpdateStatusCommand: IRequest<GenericResponse<StatusDto>>
    {
        [JsonIgnore]
        public int Id {  get; set; }
        public string NewStatusName { get; set; }
    }
}
