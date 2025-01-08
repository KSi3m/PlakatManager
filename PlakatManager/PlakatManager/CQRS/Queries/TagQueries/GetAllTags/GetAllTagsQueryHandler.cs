using AutoMapper;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Queries.TagQueries.GetAllTags
{
    public class GetAllTagsQueryHandler: IRequestHandler<GetAllTagsQuery,GenericResponseWithList<TagDto>>
    {

        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;

        public GetAllTagsQueryHandler(ElectionMaterialManagerContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<GenericResponseWithList<TagDto>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
        {
            var response = new GenericResponseWithList<TagDto>() { Data = [], Success = false, StatusCode = 400 };
            try
            {
                var tags = await _db.Tags.ToListAsync();

                if (tags == null)
                {
                    response.Message = "Tags not found.";
                    response.StatusCode = 404;
                    return response;
                }
                response.Success = true;
                response.StatusCode = 200;
                response.Data = _mapper.Map<IEnumerable<TagDto>>(tags);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;

            }
            return response;
        }
    }
}
