using AutoMapper;
using ElectionMaterialManager.AppUserContext;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.AddCommentToElectionItem
{
    public class AddCommentToElectionItemCommandHandler : IRequestHandler<AddCommentToElectionItemCommand,GenericResponse<UserCommentDto>>
    {
        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public AddCommentToElectionItemCommandHandler(ElectionMaterialManagerContext db, IMapper mapper, IUserContext userContext)
        {
            _db = db;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<GenericResponse<UserCommentDto>> Handle(AddCommentToElectionItemCommand request, CancellationToken cancellationToken)
        {
            var response = new GenericResponse<UserCommentDto>() { Success = false };
            try
            {
                var currentUser = await _userContext.GetCurrentUser();
                bool isEditable = currentUser != null;
                if (!isEditable)
                {
                    response.Message = "NOT AUTHORIZED";
                    return response;
                }

                var electionItemId = await _db.ElectionItems
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync(x => x == request.Id)
                    ;

                if (electionItemId == 0)
                {
                    response.Message = "Election item not found";
                    return response;
                }

                var comment = new Comment()
                {
                    Message = request.Message,
                    AuthorId = currentUser.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    ElectionItemId = electionItemId
                };

                _db.Add(comment);
                await _db.SaveChangesAsync();
                response.Success = true;
                response.Data = _mapper.Map<UserCommentDto>(comment);
                response.Message = $"/api/v1/election-item/{electionItemId}/comments";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
