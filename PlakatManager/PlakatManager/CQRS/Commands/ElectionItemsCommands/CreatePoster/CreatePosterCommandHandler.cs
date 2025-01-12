using AutoMapper;
using ElectionMaterialManager.AppUserContext;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using ElectionMaterialManager.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreatePoster
{
    public class CreatePosterCommandHandler : IRequestHandler<CreatePosterCommand, Response>
    {
        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly IDistrictLocalizationService _districtLocalizationService;


        public CreatePosterCommandHandler(ElectionMaterialManagerContext db, IMapper mapper,
            IUserContext userContext, IDistrictLocalizationService districtLocalizationService)
        {
            _db = db;
            _mapper = mapper;
            _userContext = userContext;
            _districtLocalizationService = districtLocalizationService;
        }

        public async Task<Response> Handle(CreatePosterCommand request, CancellationToken cancellationToken)
        {
            var response = new Response() { Success = false, StatusCode = 400 };
            try
            {
                var currentUser = await _userContext.GetCurrentUser();
                bool isEditable = currentUser != null;
                if (!isEditable)
                {
                    response.Message = "User is not authorized to access";
                    response.StatusCode = 401;
                    return response;
                }
                var tags = await _db.Tags.Where(x => request.Tags.Contains(x.Id)).ToListAsync();
                if (!tags.Any() || tags.Count() != request.Tags.Count())
                {
                    response.Message = "Tags not specified/wrong ids. Process aborted";
                    response.StatusCode = 400;
                    return response;
                }


                if (request.Location.District == null)
                {
                    if (_districtLocalizationService.GetDistrict(out string name, out string city, request.Location.Longitude, request.Location.Latitude))
                        request.Location.District = name;
                    if (request.Location.City == null) request.Location.City = city;
                }

                var poster = _mapper.Map<Poster>(request);
                poster.AuthorId = currentUser.Id;


                var electionItemTags = tags.Select(tag => new ElectionItemTag
                {
                    ElectionItem = poster,
                    Tag = tag,
                    DateOfPublication = DateTime.UtcNow
                }); ;

                await _db.AddRangeAsync(electionItemTags);
                await _db.SaveChangesAsync();
                response.Success = true;
                response.StatusCode = 201;
                // response.Data = _mapper.Map<ElectionItemDto>(poster);
                response.Message = $"/api/v1/election-item/{poster.Id}";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
