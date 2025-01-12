using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Entities;
using ElectionMaterialManager.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ElectionMaterialManager.CQRS.Commands.AuthenticationCommands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _authService;

        public LoginCommandHandler(UserManager<User> userManager, IAuthService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }

        public async Task<TokenResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var response = new TokenResponse() { Success = false, StatusCode = 400 };
            try
            {
                var user = await _userManager.FindByNameAsync(request.Login);
                if (user == null)
                {
                    response.Message = "Wrong login or password ";
                    response.StatusCode = 401;
                    return response;
                }
                var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
                if (!passwordValid)
                {
                    response.Message = "Wrong login or password";
                    response.StatusCode = 401;
                    return response;
                }
                string token = await _authService.CreateToken(user);
                response.Success = true;
                response.Token = token;
                response.StatusCode = 200;
                response.Message = "Token generated successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
