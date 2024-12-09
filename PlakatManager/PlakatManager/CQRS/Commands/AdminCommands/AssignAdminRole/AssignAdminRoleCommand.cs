using ElectionMaterialManager.CQRS.Responses;
using MediatR;

namespace ElectionMaterialManager.CQRS.Commands.AdminCommands.AssignAdminRole
{
    public class AssignAdminRoleCommand: IRequest<Response>
    {
        public string UserId {get;set;}


    }
}
