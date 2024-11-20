using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItems
{
    public class GetElectionItemsQueryHandler : IRequestHandler<GetElectionItemsQuery, GenericResponseWithList<ElectionItem>>
    {

        private readonly ElectionMaterialManagerContext _db;

        public GetElectionItemsQueryHandler(ElectionMaterialManagerContext db)
        {
            _db = db;
        }

        public async Task<GenericResponseWithList<ElectionItem>> Handle(GetElectionItemsQuery request, CancellationToken cancellationToken)
        {
            var response = new GenericResponseWithList<ElectionItem>() { Data = [], Success = false };
            try
            {
                var electionItems = await _db.ElectionItems
                   .Where(x => x.Id >= request.IndexRangeStart && x.Id <= request.IndexRangeEnd)
                   .ToListAsync();

                if (electionItems == null)
                {
                    response.Message = "Election items within given range not found.";
                    return response;
                }
                response.Message = "Election items within given range found.";
                response.Success = true;
                response.Data = electionItems;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;

            }
            return response;
        }
    }
}
