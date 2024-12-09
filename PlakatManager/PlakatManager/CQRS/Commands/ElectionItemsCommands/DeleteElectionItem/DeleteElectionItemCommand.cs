using ElectionMaterialManager.CQRS.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.DeleteElectionItem
{
    public class DeleteElectionItemCommand: IRequest<Response>
    {
  
        public int Id { get; set; }

    }
}
