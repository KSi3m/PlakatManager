using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ElectionMaterialManager.CQRS.Queries.TagQueries.GetTagById
{


    public class GetTagByIdQueryHandler: IRequestHandler<GetTagByIdQuery,GenericResponse<Tag>>
    {

        private readonly ElectionMaterialManagerContext _db;

        public GetTagByIdQueryHandler(ElectionMaterialManagerContext db)
        {
            _db = db;
        }

        public async Task<GenericResponse<Tag>> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new GenericResponse<Tag>() { Success = false };
            try
            {
                var tags = await _db.Tags.Include(x=>x.ElectionItems).FirstOrDefaultAsync(x => x.Id == request.Id);
                if (tags == null)
                {
                    response.Message = $"Tag not found";
                    return response;
                }
                response.Message = $"Tag found";
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
