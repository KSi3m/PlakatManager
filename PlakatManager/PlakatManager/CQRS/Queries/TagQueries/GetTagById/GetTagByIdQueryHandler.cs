using AutoMapper;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ElectionMaterialManager.CQRS.Queries.TagQueries.GetTagById
{


    public class GetTagByIdQueryHandler: IRequestHandler<GetTagByIdQuery,GenericResponse<TagDto>>
    {

        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;

        public GetTagByIdQueryHandler(ElectionMaterialManagerContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<GenericResponse<TagDto>> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new GenericResponse<TagDto>() { Success = false, StatusCode = 400 };
            try
            {
                var tag = await _db.Tags.Include(x=>x.ElectionItems)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);
                if (tag == null)
                {
                    response.Message = $"Tag not found";
                    response.StatusCode = 404;
                    return response;
                }
                response.Message = $"Tag found";
                response.Success = true;
                response.StatusCode = 200;
                response.Data = _mapper.Map<TagDto>(tag);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
           
        }
    }
}
