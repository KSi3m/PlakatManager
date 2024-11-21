using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Entities;
using MediatR;

namespace ElectionMaterialManager.CQRS.Commands.TagCommands.CreateTag
{
    public class CreateTagCommand: IRequest<GenericResponse<Tag>>
    {
        public string TagName {  get; set; }
    }
}
