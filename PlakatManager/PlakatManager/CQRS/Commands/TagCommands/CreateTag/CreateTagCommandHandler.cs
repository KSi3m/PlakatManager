using AutoMapper;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateBillboard;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Commands.TagCommands.CreateTag
{
    public class CreateTagCommandHandler: IRequestHandler<CreateTagCommand, GenericResponse<TagDto>>
    {
        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;

        public CreateTagCommandHandler(ElectionMaterialManagerContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<GenericResponse<TagDto>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var response = new GenericResponse<TagDto>() { Success = false };
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
                response.Data = _mapper.Map<TagDto>(tag);
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
