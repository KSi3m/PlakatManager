using ElectionMaterialManager.CQRS.Commands.AuthenticationCommands.Login;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Entities;
using ElectionMaterialManager.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
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
            var response = new Response() { Success = false, StatusCode = 400 };
            try
            {
                var userFromDb = await _userManager.FindByNameAsync(request.Username);
                if (userFromDb != null)
                {
                    response.Message = "User already exists";
                    response.StatusCode = 409;
                    return response;
                }
                var emailUnique = await _userManager.FindByEmailAsync(request.Email);
                if (emailUnique!=null)
                {
                    response.Message = "Email already used";
                    response.StatusCode = 409;
                    return response;
          
                }
                var user = new User { UserName = request.Username, Email = request.Email };
                var created = await _userManager.CreateAsync(user, request.Password);
                if (!created.Succeeded)
                {
                    response.Message = created.Errors.FirstOrDefault().Description;
                    response.StatusCode = 400;
                    return response;
                }

                response.Success = true;
                response.Message = "User created";
                response.StatusCode = 201;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;


            
        }
    }
}
