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
                var electionItem =  await _db.ElectionItems.Include(x => x.Status)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                if (electionItem == null)
                {
                    response.Message = $"Election item with id {request.Id} not found";
                    return response;
                }
                response.Message = $"Election item with id {request.Id} found";
                response.Success = true;
                response.Data = _mapper.Map<ElectionItemDto>(electionItem);
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
