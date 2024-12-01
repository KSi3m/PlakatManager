using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Runtime.Intrinsics.X86;

namespace ElectionMaterialManager.CQRS.Commands.AdminCommands.AssignAdminRole
{
    public class AssignAdminRoleCommandHandler: IRequestHandler<AssignAdminRoleCommand,Response>
    {
        private readonly UserManager<User> _userManager;

        public AssignAdminRoleCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response> Handle(AssignAdminRoleCommand request, CancellationToken cancellationToken)
        {
            var response = new Response() { Success = false };
            try
            {
                var userFromDb = await _userManager.FindByIdAsync(request.UserId);
                if (userFromDb == null)
                {
                    response.Message = "User doesn't exists";
                    return response;
                }
                if(await _userManager.IsInRoleAsync(userFromDb, "Admin"))
                {
                    response.Message = "User has the admin role already!";
                    return response;
                }
                 await _userManager.AddToRoleAsync(userFromDb, "Admin");

                response.Success = true;
                response.Message = "Admin role assigned to created";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
