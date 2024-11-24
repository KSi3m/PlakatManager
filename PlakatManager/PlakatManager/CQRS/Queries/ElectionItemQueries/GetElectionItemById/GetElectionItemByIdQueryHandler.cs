using AutoMapper;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemById
{
    public class GetElectionItemByIdQueryHandler : IRequestHandler<GetElectionItemByIdQuery, GenericResponse<ElectionItemDto>>
    {
        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;

        public GetElectionItemByIdQueryHandler(ElectionMaterialManagerContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<GenericResponse<ElectionItemDto>> Handle(GetElectionItemByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new GenericResponse<ElectionItemDto>() { Success  = false };
            try
            {
                var query =  _db.ElectionItems.Include(x => x.Status).AsQueryable();
                if (request.Detailed)
                {
                    query = query.Include(x => x.Tags)
                    .Include(x => x.Author);
                    //.Include(x => x.Comments);
                }

                var electionItem = await query.FirstOrDefaultAsync(x => x.Id == request.Id);

                if (electionItem == null)
                {
                    response.Message = $"Election item with id {request.Id} not found";
                    return response;
                }
                response.Message = $"Election item with id {request.Id} found";
                response.Success = true;
                if (request.Detailed)
                {
                    var xd = _mapper.Map<ElectionItemDetailDto>(electionItem);
                    response.Data = xd;
                    response.Message = $"Election item with id {request.Id} found XDD";
                }
                else response.Data = _mapper.Map<ElectionItemDto>(electionItem);

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
