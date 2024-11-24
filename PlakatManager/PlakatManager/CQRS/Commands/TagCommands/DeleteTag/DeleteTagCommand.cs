using ElectionMaterialManager.CQRS.Responses;
using MediatR;

namespace ElectionMaterialManager.CQRS.Commands.TagCommands.DeleteTag
{
    public class DeleteTagCommand: IRequest<Response>
    {
        public int Id {  get; set; }
    }
}
