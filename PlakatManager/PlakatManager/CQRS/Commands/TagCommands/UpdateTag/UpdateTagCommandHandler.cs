using AutoMapper;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Commands.TagCommands.UpdateTag
{
    public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand,GenericResponse<TagDto>>
    {
        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;

        public UpdateTagCommandHandler(ElectionMaterialManagerContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<GenericResponse<TagDto>> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var response = new GenericResponse<TagDto>() { Success = false };
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
                response.Data = _mapper.Map<TagDto>(tagFromDb);
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
