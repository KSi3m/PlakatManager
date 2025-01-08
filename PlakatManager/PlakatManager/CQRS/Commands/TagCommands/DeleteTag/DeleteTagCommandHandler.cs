using ElectionMaterialManager.AppUserContext;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Commands.TagCommands.DeleteTag
{
    public class DeleteTagCommandHandler: IRequestHandler<DeleteTagCommand,Response>
    {
        private readonly ElectionMaterialManagerContext _db;
        private readonly IUserContext _userContext;
        public DeleteTagCommandHandler(ElectionMaterialManagerContext db, IUserContext userContext)
        {
            _db = db;
            _userContext = userContext;
        }

        public async Task<Response> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            var response = new Response() { Success = false, StatusCode = 400 };
            try
            {
                var currentUser = await _userContext.GetCurrentUser();
                bool isEditable = currentUser != null && currentUser.Roles.Contains("Admin");
                if (!isEditable)
                {
                    response.Message = "NOT AUTHORIZED";
                    response.StatusCode = 401;
                    return response;
                }

                var tag = await _db.Tags.FirstAsync(x => x.Id.Equals(request.Id));

                if (tag == null)
                {
                    response.Message = "Tag with given id not found.";
                    response.StatusCode = 404;
                    return response;
                }

                var check = _db.ElectionItems.Where(x=>x.Tags.Contains(tag)).Count() > 0;
                if (check)
                {
                    response.Message = "Tag is being used. Use different endpoint (to do)";
                    response.StatusCode = 409;
                    return response;
                }

                _db.Remove(tag);
                await _db.SaveChangesAsync();
                response.Success = true;
                response.StatusCode = 204;
                response.Message = "Tag deleted.";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
