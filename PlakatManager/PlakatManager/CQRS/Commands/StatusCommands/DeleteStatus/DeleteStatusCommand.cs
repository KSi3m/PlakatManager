using ElectionMaterialManager.CQRS.Responses;
using MediatR;

namespace ElectionMaterialManager.CQRS.Commands.StatusCommands.DeleteStatus
{
    public class DeleteStatusCommand: IRequest<Response>
    {
        public int Id { get; set; }
    }
}
