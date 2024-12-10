using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetNearbyElectionItems
{
    public class GetNearbyElectionItemsQueryHandler: IRequestHandler<GetNearbyElectionItemsQuery, GenericResponseWithList<ElectionItemDto>>
    {
        private readonly ElectionMaterialManagerContext _db;

        public GetNearbyElectionItemsQueryHandler(ElectionMaterialManagerContext db)
        {
            _db = db;
        }

        public async Task<GenericResponseWithList<ElectionItemDto>> Handle(GetNearbyElectionItemsQuery request, CancellationToken cancellationToken)
        {
            var response = new GenericResponseWithList<ElectionItemDto>() { Data = [], Success = false };
            try
            {
                const double EarthRadiusKm = 6371.0;

                double radiusInRadians = request.RadiusInKm / EarthRadiusKm;

                double minLat = request.Latitude - radiusInRadians * (180 / Math.PI);
                double maxLat = request.Latitude + radiusInRadians * (180 / Math.PI);

                double minLon = request.Longitude - radiusInRadians * (180 / Math.PI) / Math.Cos(request.Latitude * Math.PI / 180);
                double maxLon = request.Longitude + radiusInRadians * (180 / Math.PI) / Math.Cos(request.Latitude * Math.PI / 180);



                var electionItems = await _db.ElectionItems
                   .Include(x => x.Status)
                   .Where(x => x.Location.Longitude >= minLon && x.Location.Longitude <= maxLon)
                   .Where(x => x.Location.Latitude >= minLat && x.Location.Longitude <= maxLat)
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
                       Type = EF.Property<string>(x, "Discriminator"),
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
