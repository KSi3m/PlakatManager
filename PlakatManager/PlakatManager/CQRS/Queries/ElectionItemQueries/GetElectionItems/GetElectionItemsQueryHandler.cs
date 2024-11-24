using AutoMapper;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItems
{
    public class GetElectionItemsQueryHandler : IRequestHandler<GetElectionItemsQuery, GenericResponseWithList<ElectionItemDto>>
    {

        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;

        public GetElectionItemsQueryHandler(ElectionMaterialManagerContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<GenericResponseWithList<ElectionItemDto>> Handle(GetElectionItemsQuery request, CancellationToken cancellationToken)
        {
            var response = new GenericResponseWithList<ElectionItemDto>() { Data = [], Success = false };
            try
            {
                var electionItems = await _db.ElectionItems
                   .Include(x=>x.Status)
                   .Where(x => x.Id >= request.IndexRangeStart && x.Id <= request.IndexRangeEnd)
                   .ToListAsync();

                if (electionItems == null)
                {
                    response.Message = "Election items within given range not found.";
                    return response;
                }
                response.Message = "Election items within given range found.";
                response.Success = true;
                response.Data = _mapper.Map<IEnumerable<ElectionItemDto>>(electionItems);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;

            }
            return response;
        }
    }
}
