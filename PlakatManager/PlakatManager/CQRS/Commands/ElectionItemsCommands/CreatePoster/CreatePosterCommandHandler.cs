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
    public class CreatePosterCommandHandler : IRequestHandler<CreatePosterCommand, GenericResponse<ElectionItemDto>>
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

        public async Task<GenericResponse<ElectionItemDto>> Handle(CreatePosterCommand request, CancellationToken cancellationToken)
        {
            var response = new GenericResponse<ElectionItemDto>() { Success = false };
            try
            {
                var currentUser = await _userContext.GetCurrentUser();
                bool isEditable = currentUser != null;
                if (!isEditable)
                {
                    response.Message = "NOT AUTHORIZED";
                    return response;
                }
                var tags = await _db.Tags.Where(x => request.Tags.Contains(x.Id)).ToListAsync();
                if (!tags.Any() || tags.Count() != request.Tags.Count())
                {
                    response.Message = "Tags not specified/wrong ids. Process aborted";
                    return response;
                }

                var poster = _mapper.Map<Poster>(request);
                poster.AuthorId = currentUser.Id;

                var location = _mapper.Map<Location>(request.Location);
                if (location.District == null)
                {
                    if (_districtLocalizationService.GetDistrict(out string name, location.Longitude_2, location.Latitude_2))
                        location.District = name;
                }

                poster.Location = location;

                var electionItemTags = tags.Select(tag => new ElectionItemTag
                {
                    ElectionItem = poster,
                    Tag = tag,
                    DateOfPublication = DateTime.UtcNow
                }); ;

                await _db.AddRangeAsync(electionItemTags);
                await _db.SaveChangesAsync();
                response.Success = true;
                response.Data = _mapper.Map<ElectionItemDto>(poster);
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
