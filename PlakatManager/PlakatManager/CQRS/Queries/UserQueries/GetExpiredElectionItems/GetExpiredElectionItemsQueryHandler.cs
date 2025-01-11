using AutoMapper;
using ElectionMaterialManager.AppUserContext;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Queries.UserQueries.GetExpiredElectionItems
{
    public class GetExpiredElectionItemsQueryHandler: IRequestHandler<GetExpiredElectionItemsQuery,GenericResponseWithList<ElectionItemDto>>
    {
        private readonly ElectionMaterialManagerContext _db;
        private readonly IUserContext _userContext;

        public GetExpiredElectionItemsQueryHandler(ElectionMaterialManagerContext db, IUserContext userContext)
        {
            _db = db;
            _userContext = userContext;
        }

        public async Task<GenericResponseWithList<ElectionItemDto>> Handle(GetExpiredElectionItemsQuery request, CancellationToken cancellationToken)
        {
            var response = new GenericResponseWithList<ElectionItemDto>() { Data = [], Success = false, StatusCode = 400 };
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
                        response.Message = "User is not authorized to access";
                        response.StatusCode = 401;
                        return response;
                    }
                    electionItemsQuery.Where(x => x.AuthorId == currentUser.Id);
                }


                var dateToday = DateTime.Today;
                var electionItems = await
                    electionItemsQuery
                  .Where(x => x.EndDate < dateToday)
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
                   .ToListAsync();

                /*if (electionItems == null)
                {
                    response.Message = "Election items within given priority range not found.";
                    return response;
                }*/
                response.Message = "Election items within given priority range found.";
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
