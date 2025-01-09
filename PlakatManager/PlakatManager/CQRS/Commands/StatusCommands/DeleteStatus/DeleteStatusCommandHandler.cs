using ElectionMaterialManager.AppUserContext;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Commands.StatusCommands.DeleteStatus
{
    public class DeleteStatusCommandHandler: IRequestHandler<DeleteStatusCommand,Response>
    {
        private readonly ElectionMaterialManagerContext _db;
        private readonly IUserContext _userContext;

        public DeleteStatusCommandHandler(ElectionMaterialManagerContext db, IUserContext userContext)
        {
            _db = db;
            _userContext = userContext;
        }

        public async Task<Response> Handle(DeleteStatusCommand request, CancellationToken cancellationToken)
        {
            var response = new Response() { Success = false,StatusCode = 400 };
            try
            {
                var currentUser = await _userContext.GetCurrentUser();
                bool isEditable = currentUser != null && currentUser.Roles.Contains("Admin");
                if (!isEditable)
                {
                    response.Message = "User is not authorized to access";
                    response.StatusCode = 401;
                    return response;
                }

                var status = await _db.Statuses
                    .FirstOrDefaultAsync(x => x.Id.Equals(request.Id));

                if (status == null)
                {
                    response.Message = "Status with given id not found.";
                    response.StatusCode = 404;
                    return response;
                }

                _db.Remove(status);
                await _db.SaveChangesAsync();
                response.Success = true;
                response.StatusCode = 204;
                response.Message = "Status deleted.";

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
