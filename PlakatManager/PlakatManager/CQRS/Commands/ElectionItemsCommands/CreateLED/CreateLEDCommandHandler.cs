using AutoMapper;
using ElectionMaterialManager.AppUserContext;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using ElectionMaterialManager.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateLED
{
    public class CreateLEDCommandHandler : IRequestHandler<CreateLEDCommand, GenericResponse<ElectionItemDto>>
    {

        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly IDistrictLocalizationService _districtLocalizationService;


        public CreateLEDCommandHandler(ElectionMaterialManagerContext db, IMapper mapper,
            IUserContext userContext, IDistrictLocalizationService districtLocalizationService)
        {
            _db = db;
            _mapper = mapper;
            _userContext = userContext;
            _districtLocalizationService = districtLocalizationService;

        }

        public async Task<GenericResponse<ElectionItemDto>> Handle(CreateLEDCommand request, CancellationToken cancellationToken)
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

                if (request.Location.District == null)
                {
                    if (_districtLocalizationService.GetDistrict(out string name, out string city, request.Location.Longitude, request.Location.Latitude))
                        request.Location.District = name;
                    if (request.Location.City == null) request.Location.City = city;
                }

                var led = _mapper.Map<LED>(request);
                led.AuthorId = currentUser.Id;

                var electionItemTags = tags.Select(tag => new ElectionItemTag
                {
                    ElectionItem = led,
                    Tag = tag,
                    DateOfPublication = DateTime.UtcNow
                }); ;

                await _db.AddRangeAsync(electionItemTags);
                await _db.SaveChangesAsync();
                response.Success = true;
                response.Data = _mapper.Map<ElectionItemDto>(led);
                response.Message = $"/api/v1/election-item/{led.Id}";
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
