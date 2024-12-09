using AutoMapper;
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

        public CreateStatusCommandHandler(ElectionMaterialManagerContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<GenericResponse<StatusDto>> Handle(CreateStatusCommand request, CancellationToken cancellationToken)
        {
            var response = new GenericResponse<StatusDto>() { Success = false };
            try
            {
                var statusFromDb = await _db.Statuses                    
                .FirstOrDefaultAsync(x => x.Name.ToLower() == request.Name.ToLower());

                if (statusFromDb != null)
                {
                    response.Message = "Status already exists";
                    return response;
                }

                var status = new Status()
                {
                    Name = request.Name,
                };

                _db.Statuses.Add(status);
                await _db.SaveChangesAsync();

                response.Success = true;
                response.Data = _mapper.Map<StatusDto>(status);
                response.Message = $"/api/v1/status/{status.Id}";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
