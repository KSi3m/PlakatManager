using ElectionMaterialManager.AppUserContext;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetSoonExpiringElectionItems
{
    public class GetSoonExpiringElectionItemsQueryHandler : IRequestHandler<GetSoonExpiringElectionItemsQuery, GenericResponseWithList<ElectionItemDto>>
    {

        private readonly ElectionMaterialManagerContext _db;
        private readonly IUserContext _userContext;

        public GetSoonExpiringElectionItemsQueryHandler(ElectionMaterialManagerContext db, IUserContext userContext)
        {
            _db = db;
            _userContext = userContext;
        }

        public async Task<GenericResponseWithList<ElectionItemDto>> Handle(GetSoonExpiringElectionItemsQuery request, CancellationToken cancellationToken)
        {
            var response = new GenericResponseWithList<ElectionItemDto>() { Data = [], Success = false };
            try
            {

                var electionItemsQuery = _db.ElectionItems
                    .Include(x => x.Status).AsQueryable();


                if (request.UserOnly)
                {

                    var currentUser = await _userContext.GetCurrentUser();
                    bool isEnterable = currentUser != null;
                    if (!isEnterable)
                    {
                        response.Message = "NOT AUTHORIZED";
                        return response;
                    }
                    electionItemsQuery.Where(x => x.AuthorId == currentUser.Id);
                }
                /*
                

                var electionItems = await
                    electionItemsQuery
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
                   .OrderByDescending(x => x.Priority)
                   .ToListAsync();*/

                /*if (electionItems == null)
                {
                    response.Message = "Election items within given priority range not found.";
                    return response;
                }*/
                response.Message = "Election items within given priority range found.";
                response.Success = true;
                //response.Data = electionItems;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;

            }
            return response;
        }
    }
}
