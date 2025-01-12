using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemsByDistrict
{
    public class GetElectionItemsByDistrictQueryHandler: IRequestHandler<GetElectionItemsByDistrictQuery, GenericResponseWithList<ElectionItemDto>>
    {
        private readonly ElectionMaterialManagerContext _db;

        public GetElectionItemsByDistrictQueryHandler(ElectionMaterialManagerContext db)
        {
            _db = db;
        }

        public async Task<GenericResponseWithList<ElectionItemDto>> Handle(GetElectionItemsByDistrictQuery request, CancellationToken cancellationToken)
        {
            var response = new GenericResponseWithList<ElectionItemDto>() { Data = [], Success = false,StatusCode = 400 };
            try
            {

                var electionItems = await _db.ElectionItems
                    .Include(x => x.Status)
                    .Where(x => x.Location.District == request.District)
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
                        Priority = x.Priority,
                        Type = EF.Property<string>(x, "Discriminator"),
                    })
                    .ToListAsync();

                response.Message = "Election items from within given district tag found";
                response.Success = true;
                response.StatusCode = 200;

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
