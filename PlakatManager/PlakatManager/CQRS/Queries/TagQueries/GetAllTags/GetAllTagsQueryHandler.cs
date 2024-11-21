using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Queries.TagQueries.GetAllTags
{
    public class GetAllTagsQueryHandler: IRequestHandler<GetAllTagsQuery,GenericResponseWithList<Tag>>
    {

        private readonly ElectionMaterialManagerContext _db;

        public GetAllTagsQueryHandler(ElectionMaterialManagerContext db)
        {
            _db = db;
        }

        public async Task<GenericResponseWithList<Tag>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
        {
            var response = new GenericResponseWithList<Tag>() { Data = [], Success = false };
            try
            {
                var tags = await _db.Tags.ToListAsync();

                if (tags == null)
                {
                    response.Message = "Tags not found.";
                    return response;
                }
                response.Success = true;
                response.Data = tags;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;

            }
            return response;
        }
    }
}
