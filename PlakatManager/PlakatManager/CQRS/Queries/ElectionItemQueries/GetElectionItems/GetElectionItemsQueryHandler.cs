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
                   .Include(x => x.Status)
                   .Where(x => x.Id >= request.IndexRangeStart && x.Id <= request.IndexRangeEnd)
                   .Select(x => new ElectionItemDto()
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
                       Type =  EF.Property<string>(x, "Discriminator"),
                       Priority = x.Priority
                   })
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
