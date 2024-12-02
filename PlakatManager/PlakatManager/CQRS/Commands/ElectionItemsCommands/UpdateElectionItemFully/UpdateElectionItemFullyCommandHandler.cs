﻿using AutoMapper;
using ElectionMaterialManager.AppUserContext;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.UpdateElectionItemPartially;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.UpdateElectionItemFully
{
    public class UpdateElectionItemFullyCommandHandler: IRequestHandler<UpdateElectionItemFullyCommand, Response>
    {
        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public UpdateElectionItemFullyCommandHandler(ElectionMaterialManagerContext db, IMapper mapper, IUserContext userContext)
        {
            _db = db;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<Response> Handle(UpdateElectionItemFullyCommand request, CancellationToken cancellationToken)
        {
            var response = new Response() { Success = false };
            try
            {
                var item = await _db.ElectionItems.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (item == null)
                {
                    response.Message = "Item not found";
                    return response;
                }


                var currentUser = await _userContext.GetCurrentUser();
                bool isEditable = currentUser != null && 
                    (item.AuthorId == currentUser.Id || currentUser.Roles.Contains("Admin"));
                if (!isEditable)
                {
                    response.Message = "NOT AUTHORIZED";
                    return response;
                }
                _mapper.Map(request, item);
                if (request.Tags != null && request.Tags.Any())
                {
                    var tags = await _db.Tags.Where(x => request.Tags.Contains(x.Id)).ToListAsync();
                    if (!tags.Any() || tags.Count != request.Tags.Count())
                    {
                        response.Message = "Tags not specified/wrong ids. Process aborted";
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
