using Azure;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemsByTag
{
    public class GetElectionItemsByTagQueryHandler : IRequestHandler<GetElectionItemsByTagQuery, GenericResponseWithList<ElectionItem>>
    {

        private readonly ElectionMaterialManagerContext _db;

        public GetElectionItemsByTagQueryHandler(ElectionMaterialManagerContext db)
        {
            _db = db;
        }

        public async Task<GenericResponseWithList<ElectionItem>> Handle(GetElectionItemsByTagQuery query, CancellationToken cancellationToken)
        {

            var response = new GenericResponseWithList<ElectionItem>() { Data = [], Success = false };
            try
            {
                var electionItems = await _db.ElectionItems
                    .Include(x => x.Tags)
                    .Where(x => x.Tags.Any(y => y.Value == query.TagName))
                    .ToListAsync();

                if (electionItems == null || electionItems.Any())
                {
                    response.Message = "Election items with given tag not found.";
                    return response;
                }
                response.Message = "Election items with given tag found";
                response.Success = true;
                response.Data = electionItems;
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
            

        }
    }
}
