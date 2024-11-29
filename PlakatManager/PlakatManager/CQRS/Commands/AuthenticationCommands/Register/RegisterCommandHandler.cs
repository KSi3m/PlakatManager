using ElectionMaterialManager.CQRS.Commands.AuthenticationCommands.Login;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Entities;
using ElectionMaterialManager.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace ElectionMaterialManager.CQRS.Commands.AuthenticationCommands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Response>
    {
        private readonly UserManager<User> _userManager;

        public RegisterCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var response = new Response() { Success = false };
            try
            {
                var userFromDb = await _userManager.FindByNameAsync(request.Username);
                if (userFromDb != null)
                {
                    response.Message = "User already exists";
                    return response;
                }
                var emailUnique = await _userManager.FindByEmailAsync(request.Email);
                if (emailUnique!=null)
                {
                    response.Message = "Email already used";
                    return response;
          
                }
                var user = new User { UserName = request.Username, Email = request.Email };
                var created = await _userManager.CreateAsync(user, request.Password);
                if (!created.Succeeded)
                {
                    response.Message = created.Errors.FirstOrDefault().Description;
                    return response;
                }

                response.Success = true;
                response.Message = "User created";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;


            
        }
    }
}
