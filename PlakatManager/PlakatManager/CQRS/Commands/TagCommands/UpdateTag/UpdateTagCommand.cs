using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using MediatR.NotificationPublishers;
using System.Text.Json.Serialization;

namespace ElectionMaterialManager.CQRS.Commands.TagCommands.UpdateTag
{
    public class UpdateTagCommand : IRequest<GenericResponse<TagDto>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string NewTagName { get; set; }
    }
}
