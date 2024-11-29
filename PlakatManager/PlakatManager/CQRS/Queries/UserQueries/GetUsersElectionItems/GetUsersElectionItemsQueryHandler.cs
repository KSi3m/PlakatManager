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
                var currentUser = await _userContext.GetCurrentIdentityUser();
                bool isEditable = currentUser != null;
                if (!isEditable)
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
                       Latitude = x.Latitude,
                       Longitude = x.Longitude,
                       Priority = x.Priority 
                      })
                    .ToListAsync();

                /*if (electionItems == null)
                {
                    response.Message = "Items not found.";
                    return response;
                }*/
                response.Success = true;
                //response.Data = _mapper.Map<IEnumerable<ElectionItemDto>>(electionItems);
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
