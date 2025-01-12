using AutoMapper;
using ElectionMaterialManager.AppUserContext;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Commands.StatusCommands.CreateStatus
{
    public class CreateStatusCommandHandler: IRequestHandler<CreateStatusCommand,GenericResponse<StatusDto>>
    {

        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public CreateStatusCommandHandler(ElectionMaterialManagerContext db, IMapper mapper, IUserContext userContext)
        {
            _db = db;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<GenericResponse<StatusDto>> Handle(CreateStatusCommand request, CancellationToken cancellationToken)
        {
            var response = new GenericResponse<StatusDto>() { Success = false, StatusCode = 400 };
            try
            {

                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    response.Message = "Status name cannot be empty";
                    return response;
                }

                var currentUser = await _userContext.GetCurrentUser();
                bool isEditable = currentUser != null && currentUser.Roles.Contains("Admin");
                if (!isEditable)
                {
                    response.Message = "User is not authorized to access";
                    response.StatusCode = 401;
                    return response;
                }


                var statusFromDb = await _db.Statuses                    
                .FirstOrDefaultAsync(x => x.Name.ToLower() == request.Name.ToLower());

                if (statusFromDb != null)
                {
                    response.Message = "Status already exists";
                    response.StatusCode = 409;
                    return response;
                }

                var status = new Status()
                {
                    Name = request.Name,
                };

                _db.Statuses.Add(status);
                await _db.SaveChangesAsync();

                response.Success = true;
                response.StatusCode = 201;
                response.Data = _mapper.Map<StatusDto>(status);
                response.Message = $"Status added successfully. Resource can be found at /api/v1/status/{status.Id}";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
