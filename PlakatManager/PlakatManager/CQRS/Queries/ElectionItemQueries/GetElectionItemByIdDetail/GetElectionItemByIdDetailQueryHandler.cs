using AutoMapper;
using ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemById;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemByIdDetail
{
    public class GetElectionItemByIdDetailQueryHandler : IRequestHandler<GetElectionItemByIdDetailQuery, GenericResponse<ElectionItemDetailDto>>
    {
        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;

        public GetElectionItemByIdDetailQueryHandler(ElectionMaterialManagerContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<GenericResponse<ElectionItemDetailDto>> Handle(GetElectionItemByIdDetailQuery request, CancellationToken cancellationToken)
        {
            var response = new GenericResponse<ElectionItemDetailDto>() { Success = false, StatusCode = 400 };
            try
            {
                var electionItem = await _db.ElectionItems
                    .Include(x => x.Status)
                    .Include(x => x.Tags)
                    .Include(x => x.Author)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                if (electionItem == null)
                {
                    response.Message = $"Election item with id {request.Id} not found";
                    response.StatusCode = 404;
                    return response;
                }
                response.Message = $"Election item with id {request.Id} found";
                response.Success = true;
                response.Data = _mapper.Map<ElectionItemDetailDto>(electionItem);
                response.StatusCode = 200;
                response.Data.Type = electionItem.GetType().Name;

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    
    }
}
