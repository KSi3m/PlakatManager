using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateBillboard;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Commands.TagCommands.CreateTag
{
    public class CreateTagCommandHandler: IRequestHandler<CreateTagCommand, GenericResponse<Tag>>
    {
        private readonly ElectionMaterialManagerContext _db;

        public CreateTagCommandHandler(ElectionMaterialManagerContext db)
        {
            _db = db;
        }

        public async Task<GenericResponse<Tag>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var response = new GenericResponse<Tag>() { Success = false };
            try
            {
                var tagFromDb = await _db.Tags
                .FirstOrDefaultAsync(x => x.Value.ToLower() == request.TagName.ToLower());
                if (tagFromDb != null)
                {
                    response.Message = "Tag already exists";
                    return response;
                }

                var tag = new Tag()
                {
                    Value = request.TagName,
                };

                _db.Tags.Add(tag);
                await _db.SaveChangesAsync();

                response.Success = true;
                response.Data = tag;
                response.Message = $"/api/v1/tag/{tag.Id}";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;

        }
    }
}
