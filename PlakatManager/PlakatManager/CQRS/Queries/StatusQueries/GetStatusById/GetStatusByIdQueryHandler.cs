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
            var response = new GenericResponse<StatusDto>() { Success = false, StatusCode = 400 };
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
                    response.StatusCode = 404;
                    return response;
                }

                response.Success = true;
                response.StatusCode = 200;
                response.Data = status;
                response.Message = $"Status with id {status.Id} found";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;

            }
            return response;
        }
    }
}
