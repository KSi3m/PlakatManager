using AutoMapper;
using ElectionMaterialManager.AppUserContext;
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
        private readonly IUserContext _userContext;

        public UpdateTagCommandHandler(ElectionMaterialManagerContext db, 
            IMapper mapper, IUserContext userContext)
        {
            _db = db;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<GenericResponse<TagDto>> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var response = new GenericResponse<TagDto>() { Success = false };
            try
            {
                var currentUser = await _userContext.GetCurrentUser();
                bool isEditable = currentUser != null && currentUser.Roles.Contains("Admin");
                if (!isEditable)
                {
                    response.Message = "NOT AUTHORIZED";
                    return response;
                }

                var tags = await _db.Tags.ToListAsync();
 
                var tagFromDb = tags.FirstOrDefault(x => x.Id == request.Id);
                if (tagFromDb == null)
                {
                    response.Message = "Tag was not found";
                    return response;
                }

                var check = tags.Find(x => x.Value == request.NewTagName);
                if (check != null)
                {
                    response.Message = "Tag with the same name already exist!";
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
