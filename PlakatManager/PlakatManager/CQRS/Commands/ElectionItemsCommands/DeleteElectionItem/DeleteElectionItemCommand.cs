using MediatR;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.DeleteElectionItem
{
    public class DeleteElectionItemCommand: IRequest<Response>
    {
        public int Id { get; set; }

    }
}
