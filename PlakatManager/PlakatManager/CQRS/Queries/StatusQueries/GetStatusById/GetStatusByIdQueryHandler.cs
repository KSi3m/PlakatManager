using Azure;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Queries.StatusQueries.GetStatusById
{
    public class GetStatusByIdQueryHandler: IRequestHandler<GetStatusByIdQuery,GenericResponse<StatusDto>>
    {

        private readonly ElectionMaterialManagerContext _db;

        public GetStatusByIdQueryHandler(ElectionMaterialManagerContext db)
        {
            _db = db;
        }

        public async Task<GenericResponse<StatusDto>> Handle(GetStatusByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new GenericResponse<StatusDto>() { Success = false };
            try
            {
                var status = await _db.Statuses
                    .Where(x => x.Id == request.Id)
                    .Select(x => new StatusDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                    })
                    .FirstOrDefaultAsync();

                if (status == null)
                {
                    response.Message = $"Status not found";
                    return response;
                }

                response.Success = true;
                response.Data = status;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;

            }
            return response;
        }
    }
}
