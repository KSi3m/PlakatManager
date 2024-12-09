using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using MediatR;
using System.Text.Json.Serialization;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.AddCommentToElectionItem
{
    public class AddCommentToElectionItemCommand: IRequest<GenericResponse<UserCommentDto>>
    {
        [JsonIgnore]
        public int Id {  get; set; }
        public string Message { get; set; }
    }
}
