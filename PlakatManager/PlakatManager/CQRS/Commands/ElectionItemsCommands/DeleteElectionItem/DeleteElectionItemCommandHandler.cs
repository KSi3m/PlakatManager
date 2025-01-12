using ElectionMaterialManager.AppUserContext;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.DeleteElectionItem
{
    public class DeleteElectionItemCommandHandler: IRequestHandler<DeleteElectionItemCommand,Response>
    {
        private readonly ElectionMaterialManagerContext _db;
        private readonly IUserContext _userContext;

        public DeleteElectionItemCommandHandler(ElectionMaterialManagerContext db, IUserContext userContext)
        {
            _db = db;
            _userContext = userContext;
        }

        public async Task<Response> Handle(DeleteElectionItemCommand request, CancellationToken cancellationToken)
        {

            var response = new Response() { Success = false, StatusCode = 400 };
            try
            {

                var electionItem = await _db.ElectionItems
                    .FirstOrDefaultAsync(x => x.Id.Equals(request.Id));

                if (electionItem == null)
                {
                    response.Message = "Election item with given id not found.";
                    response.StatusCode = 404;
                    return response;
                }
         
                var currentUser = await _userContext.GetCurrentUser();
                bool isEditable = currentUser != null && 
                    (electionItem.AuthorId == currentUser.Id || currentUser.Roles.Contains("Admin"));
                if (!isEditable)
                {
                    response.Message = "User is not authorized to access";
                    response.StatusCode = 401;
                    return response;
                }


                _db.Remove(electionItem);
                await _db.SaveChangesAsync();
                response.Success = true;
                response.StatusCode = 204;
                response.Message = "Election item deleted.";

            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
