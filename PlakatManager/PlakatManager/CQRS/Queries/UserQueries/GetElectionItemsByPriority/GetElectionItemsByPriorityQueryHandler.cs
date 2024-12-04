using AutoMapper;
using ElectionMaterialManager.AppUserContext;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Queries.UserQueries.GetElectionItemsByPriority
{
    public class GetElectionItemsByPriorityQueryHandler : IRequestHandler<GetElectionItemsByPriorityQuery, GenericResponseWithList<ElectionItemDto>>
    {

        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public GetElectionItemsByPriorityQueryHandler(ElectionMaterialManagerContext db,
            IMapper mapper, IUserContext userContext)
        {
            _db = db;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<GenericResponseWithList<ElectionItemDto>> Handle(GetElectionItemsByPriorityQuery request, CancellationToken cancellationToken)
        {
            var response = new GenericResponseWithList<ElectionItemDto>() { Data = [], Success = false };
            try
            {
                var currentUser = await _userContext.GetCurrentUser();
                bool isEnterable = currentUser != null;
                if (!isEnterable)
                {
                    response.Message = "NOT AUTHORIZED";
                    return response;
                }

                var electionItems = await _db.ElectionItems
                    .Include(x => x.Status)
                    .Where(x => x.AuthorId == currentUser.Id)
                   .Where(x => x.Priority >= request.MinPriority && x.Priority <= request.MaxPriority)
                   .Select(x => new ElectionItemDto()
                   {
                       Id = x.Id,
                       Area = x.Area,
                       Status = x.Status.Name,
                       Latitude = x.Latitude,
                       Longitude = x.Longitude,
                       Type = EF.Property<string>(x, "Discriminator"),
                       Priority = x.Priority
                   })
                   .OrderByDescending(x => x.Priority)
                   .ToListAsync();

                if (electionItems == null)
                {
                    response.Message = "Election items within given priority range not found.";
                    return response;
                }
                response.Message = "Election items within given priority range found.";
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
