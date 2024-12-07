using AutoMapper;
using ElectionMaterialManager.AppUserContext;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Queries.UserQueries.GetUsersElectionItems
{
    public class GetUsersElectionItemsQueryHandler: IRequestHandler<GetUsersElectionItemsQuery,GenericResponseWithList<ElectionItemDto>>
    {
        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public GetUsersElectionItemsQueryHandler(ElectionMaterialManagerContext db, 
            IMapper mapper, IUserContext userContext)
        {
            _db = db;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<GenericResponseWithList<ElectionItemDto>> Handle(GetUsersElectionItemsQuery request, CancellationToken cancellationToken)
        {
            var response = new GenericResponseWithList<ElectionItemDto>() { Data = [], Success = false };
            try
            {
                var currentUser = await _userContext.GetCurrentUser();
                bool isEnterable= currentUser != null;
                if (!isEnterable)
                {
                    response.Message = "NOT AUTHORIZED";
                    return response;
                }

                var electionItems = await _db.ElectionItems
                    .Include(x => x.Status)
                    .Where(x => x.AuthorId == currentUser.Id)
                    .Select(x => new  ElectionItemDto()
                    {
                       Id = x.Id,
                       Area = x.Area,
                       Status = x.Status.Name,
                       Location =
                       {
                           Latitude = x.Location.Latitude,
                           Longitude = x.Location.Longitude,
                           District = x.Location.District,
                           Street = x.Location.Street,
                           Description = x.Location.Description
                       },
                        Priority = x.Priority,
                       Type = EF.Property<string>(x, "Discriminator"),
                    })
                    .ToListAsync();

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
