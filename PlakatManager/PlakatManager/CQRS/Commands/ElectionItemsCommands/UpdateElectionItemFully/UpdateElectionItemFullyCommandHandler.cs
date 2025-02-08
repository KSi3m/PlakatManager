using AutoMapper;
using ElectionMaterialManager.AppUserContext;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.UpdateElectionItemPartially;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Entities;
using ElectionMaterialManager.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.UpdateElectionItemFully
{
    public class UpdateElectionItemFullyCommandHandler: IRequestHandler<UpdateElectionItemFullyCommand, Response>
    {
        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly IDistrictLocalizationService _districtLocalizationService;

        public UpdateElectionItemFullyCommandHandler(ElectionMaterialManagerContext db, 
            IMapper mapper, IUserContext userContext, IDistrictLocalizationService districtLocalizationService)
        {
            _db = db;
            _mapper = mapper;
            _userContext = userContext;
            _districtLocalizationService = districtLocalizationService;
        }

        public async Task<Response> Handle(UpdateElectionItemFullyCommand request, CancellationToken cancellationToken)
        {
            var response = new Response() { Success = false, StatusCode = 400 };
            try
            {
                var item = await _db.ElectionItems.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (item == null)
                {
                    response.Message = "Item not found";
                    response.StatusCode = 404;
                    return response;
                }


                var currentUser = await _userContext.GetCurrentUser();
                bool isEditable = currentUser != null && 
                    (item.AuthorId == currentUser.Id || currentUser.Roles.Contains("Admin"));
                if (!isEditable)
                {
                    response.Message = "User is not authorized to access";
                    response.StatusCode = 401;
                    return response;
                }

                if (request.Location != null && request.Location.District == null)
                {
                    if (_districtLocalizationService.GetDistrict(out string name, out string city, request.Location.Longitude, request.Location.Latitude))
                        request.Location.District = name;
                    if (request.Location.City == null) request.Location.City = city;
                }

                _mapper.Map(request, item);
                if (request.Tags != null && request.Tags.Any())
                {
                    var tags = await _db.Tags.Where(x => request.Tags.Contains(x.Id)).ToListAsync();
                    if (!tags.Any() || tags.Count != request.Tags.Count())
                    {
                        response.Message = "Tags not specified/wrong ids. Process aborted";
                        response.StatusCode = 400;
                        return response;
                    }

                    var existingTags = _db.ElectionItemTag.Where(et => et.ElectionItemId == item.Id);
                    _db.ElectionItemTag.RemoveRange(existingTags);

                    var newElectionItemTags = tags.Select(tag => new ElectionItemTag
                    {
                        ElectionItem = item,
                        Tag = tag,
                        DateOfPublication = DateTime.UtcNow
                    });
                    await _db.AddRangeAsync(newElectionItemTags);

                }
                _db.ElectionItems.Update(item);
                await _db.SaveChangesAsync();
                response.Success = true;
                response.StatusCode = 204;
                response.Message = "Updated succesfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
