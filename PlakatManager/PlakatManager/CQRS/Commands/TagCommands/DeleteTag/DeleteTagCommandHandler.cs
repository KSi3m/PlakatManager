using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Commands.TagCommands.DeleteTag
{
    public class DeleteTagCommandHandler: IRequestHandler<DeleteTagCommand,Response>
    {
        private readonly ElectionMaterialManagerContext _db;
        public DeleteTagCommandHandler(ElectionMaterialManagerContext db)
        {
            _db = db;
        }

        public async Task<Response> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            var response = new Response() { Success = false };
            try
            {
                var tag = await _db.Tags.FirstAsync(x => x.Id.Equals(request.Id));

                if (tag == null)
                {
                    response.Message = "Tag with given id not found.";
                    return response;
                }
                _db.Remove(tag);
                await _db.SaveChangesAsync();
                response.Success = true;
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
