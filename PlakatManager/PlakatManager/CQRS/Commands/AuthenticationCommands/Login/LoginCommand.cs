using ElectionMaterialManager.CQRS.Responses;
using MediatR;

namespace ElectionMaterialManager.CQRS.Commands.AuthenticationCommands.Login
{
    public class LoginCommand: IRequest<TokenResponse>
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
