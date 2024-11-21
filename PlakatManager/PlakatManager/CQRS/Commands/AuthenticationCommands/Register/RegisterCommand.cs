using ElectionMaterialManager.CQRS.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ElectionMaterialManager.CQRS.Commands.AuthenticationCommands.Register
{
    public class RegisterCommand: IRequest<Response>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
