﻿using AutoMapper;
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
            var response = new GenericResponseWithList<ElectionItemDto>() { Data = [], Success = false, StatusCode = 400 };
            try
            {
                var currentUser = await _userContext.GetCurrentUser();
                bool isEnterable = currentUser != null;
                if (!isEnterable)
                {
                    response.Message = "User is not authorized to access";
                    response.StatusCode = 401;
                    return response;
                }

                var electionItems = await _db.ElectionItems
                    .Include(x => x.Status)
                    .Where(x => x.AuthorId == currentUser.Id)
                   .Where(x => x.Priority >= request.MinPriority && x.Priority <= request.MaxPriority)
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
