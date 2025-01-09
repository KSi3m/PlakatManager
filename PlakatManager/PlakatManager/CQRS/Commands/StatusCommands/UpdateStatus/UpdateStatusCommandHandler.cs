using AutoMapper;
using ElectionMaterialManager.AppUserContext;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Commands.StatusCommands.UpdateStatus
{
    public class UpdateStatusCommandHandler: IRequestHandler<UpdateStatusCommand,GenericResponse<StatusDto>>
    {
        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public UpdateStatusCommandHandler(ElectionMaterialManagerContext db, 
            IMapper mapper, IUserContext userContext)
        {
            _db = db;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<GenericResponse<StatusDto>> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
        {
            var response = new GenericResponse<StatusDto>() { Success = false,StatusCode = 400 };
            try
            {
                var currentUser = await _userContext.GetCurrentUser();
                bool isEditable = currentUser != null && currentUser.Roles.Contains("Admin");
                if (!isEditable)
                {
                    response.Message = "User is not authorized to access";
                    return response;
                }

                var statuses = await _db.Statuses.ToListAsync();

                var statusFromDb = statuses
                .FirstOrDefault(x => x.Id == request.Id);
                if (statusFromDb == null)
                {
                    response.Message = "Status was not found!";
                    response.StatusCode = 404;
                    return response;
                }
                var check = statuses.Find(x => x.Name == request.NewStatusName);
                if (check != null)
                {
                    response.Message = "Status with the same name already exist!";
                    response.StatusCode = 409;
                    return response;
                }
                statusFromDb.Name = request.NewStatusName;
                await _db.SaveChangesAsync();

                response.Success = true;
                response.StatusCode = 204;
                response.Data = _mapper.Map<StatusDto>(statusFromDb);
                response.Message = $"Status updated successfully. Resource can be found at /api/v1/status/{statusFromDb.Id}";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
