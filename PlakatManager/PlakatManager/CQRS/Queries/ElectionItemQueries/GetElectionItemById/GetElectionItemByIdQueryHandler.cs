using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemById
{
    public class GetElectionItemByIdQueryHandler : IRequestHandler<GetElectionItemByIdQuery, GenericResponse<ElectionItem>>
    {
        private readonly ElectionMaterialManagerContext _db;

        public GetElectionItemByIdQueryHandler(ElectionMaterialManagerContext db)
        {
            _db = db;
        }

        public async Task<GenericResponse<ElectionItem>> Handle(GetElectionItemByIdQuery query, CancellationToken cancellationToken)
        {


            var response = new GenericResponse<ElectionItem>() { Success  = false };
            try
            {
                var electionItem = await _db.ElectionItems.FirstOrDefaultAsync(x => x.Id == query.Id);

                if (electionItem == null)
                {

                    response.Message = $"Election item with id {query.Id} not found";
                    return response;
                }
                response.Message = $"Election item with id {query.Id} found";
                response.Success = true;
                response.Data = electionItem;

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
