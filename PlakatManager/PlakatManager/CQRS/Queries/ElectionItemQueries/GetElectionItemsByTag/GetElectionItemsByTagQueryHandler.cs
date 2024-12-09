using AutoMapper;
using Azure;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemsByTag
{
    public class GetElectionItemsByTagQueryHandler : IRequestHandler<GetElectionItemsByTagQuery, GenericResponseWithList<ElectionItemDto>>
    {

        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;

        public GetElectionItemsByTagQueryHandler(ElectionMaterialManagerContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<GenericResponseWithList<ElectionItemDto>> Handle(GetElectionItemsByTagQuery query, CancellationToken cancellationToken)
        {

            var response = new GenericResponseWithList<ElectionItemDto>() { Data = [], Success = false };
            try
            {
                var electionItems = await _db.Tags.Include(x => x.ElectionItems)
                    .ThenInclude(x=>x.Status)
                    .Where(x=>x.Value == query.TagName)
                    .SelectMany(x=>x.ElectionItems)
                    .Select(x=> new ElectionItemDto()
                    {
                        Id = x.Id,
                        Status = x.Status.Name,
                        Location = new LocationDto()
                       {
                           Latitude = x.Location.Latitude,
                           Longitude = x.Location.Longitude,
                           District = x.Location.District,
                           City = x.Location.City,
                           Street = x.Location.Street,
                           Description = x.Location.Description
                       },
                        Priority = x.Priority,
                        Type = EF.Property<string>(x, "Discriminator"),
                    })
                    .ToListAsync();

               /* if (electionItems == null || !electionItems.Any())
                {
                    response.Message = "Election items with given tag not found.";
                    return response;
                }*/
                response.Message = "Election items with given tag found";
                response.Success = true;
              
                //response.Data = _mapper.Map<IEnumerable<ElectionItemDto>>(electionItems);
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
