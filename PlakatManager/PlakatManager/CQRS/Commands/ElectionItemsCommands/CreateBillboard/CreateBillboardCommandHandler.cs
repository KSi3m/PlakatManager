using AutoMapper;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateLED;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateBillboard
{
    public class CreateBillboardCommandHandler: IRequestHandler<CreateBillboardCommand, GenericResponse<ElectionItemDto>>
    {
        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;

        public CreateBillboardCommandHandler(ElectionMaterialManagerContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<GenericResponse<ElectionItemDto>> Handle(CreateBillboardCommand request, CancellationToken cancellationToken)
        {
            var response = new GenericResponse<ElectionItemDto>() { Success = false };
            try
            { 
                var tags = await _db.Tags.Where(x => request.Tags.Contains(x.Id)).ToListAsync();
                if(!tags.Any() || tags.Count() != request.Tags.Count())
                {
                    response.Message = "Tags not specified/wrong ids. Process aborted";
                    return response;
                }

                var billboard = _mapper.Map<Billboard>(request);
  

                var electionItemTags = tags.Select(tag => new ElectionItemTag
                {
                    ElectionItem = billboard,
                    Tag = tag,
                    DateOfPublication = DateTime.UtcNow
                }); ;

                await _db.AddRangeAsync(electionItemTags);
                await _db.SaveChangesAsync();
                response.Success = true;
                response.Data = _mapper.Map<ElectionItemDto>(billboard);
                response.Message = $"/api/v1/election-item/{billboard.Id}";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
