using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Commands.TagCommands.UpdateTag
{
    public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand,GenericResponse<Tag>>
    {
        private readonly ElectionMaterialManagerContext _db;

        public UpdateTagCommandHandler(ElectionMaterialManagerContext db)
        {
            _db = db;
        }

        public async Task<GenericResponse<Tag>> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var response = new GenericResponse<Tag>() { Success = false };
            try
            {
                var tagFromDb = await _db.Tags
                .FirstOrDefaultAsync(x => x.Value.ToLower() == request.OldTagName.ToLower());
                if (tagFromDb == null)
                {
                    response.Message = "Tag was not found";
                    return response;
                }
                tagFromDb.Value = request.NewTagName;
                await _db.SaveChangesAsync();

                response.Success = true;
                response.Data = tagFromDb;
                response.Message = $"/api/v1/tag/{tagFromDb.Id}";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
