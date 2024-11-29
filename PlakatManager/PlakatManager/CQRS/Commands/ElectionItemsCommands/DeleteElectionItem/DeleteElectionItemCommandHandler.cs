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

            var response = new Response() { Success = false };
            try
            {

                var electionItem = await _db.ElectionItems.FirstAsync(x => x.Id.Equals(request.Id));

                if (electionItem == null)
                {
                    response.Message = "Election item with given id not found.";
                    return response;
                }

                var currentUser = await _userContext.GetCurrentIdentityUser();
                bool isEditable = currentUser != null && electionItem.AuthorId == currentUser.Id;
                if (!isEditable)
                {
                    response.Message = "NOT AUTHORIZED";
                    return response;
                }


                _db.Remove(electionItem);
                await _db.SaveChangesAsync();
                response.Success = true;
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
