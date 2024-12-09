using AutoMapper;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Queries.StatusQueries.GetAllStatuses
{
    public class GetAllStatusesQueryHandler : IRequestHandler<GetAllStatusesQuery, GenericResponseWithList<StatusDto>>
    {

        private readonly ElectionMaterialManagerContext _db;

        public GetAllStatusesQueryHandler(ElectionMaterialManagerContext db)
        {
            _db = db;
        }

        public async Task<GenericResponseWithList<StatusDto>> Handle(GetAllStatusesQuery request, CancellationToken cancellationToken)
        {
            var response = new GenericResponseWithList<StatusDto>() { Data = [], Success = false };
            try
            {
                var statuses = await _db.Statuses
                    .Select(x=> new StatusDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                    })
                    .ToListAsync();

                response.Success = true;
                response.Data = statuses;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;

            }
            return response;
        }
    }
}
