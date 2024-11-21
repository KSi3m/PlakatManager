using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Entities;
using MediatR;
using MediatR.NotificationPublishers;

namespace ElectionMaterialManager.CQRS.Commands.TagCommands.UpdateTag
{
    public class UpdateTagCommand : IRequest<GenericResponse<Tag>>
    {
        public string NewTagName { get; set; }
        public string OldTagName { get; set; }
    }
}
