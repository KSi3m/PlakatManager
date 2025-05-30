﻿using AutoMapper;
using ElectionMaterialManager.AppUserContext;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateBillboard;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Commands.TagCommands.CreateTag
{
    public class CreateTagCommandHandler: IRequestHandler<CreateTagCommand, GenericResponse<TagDto>>
    {
        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public CreateTagCommandHandler(ElectionMaterialManagerContext db, IMapper mapper, IUserContext userContext)
        {
            _db = db;
            _mapper = mapper;
             _userContext  = userContext;
        }

        public async Task<GenericResponse<TagDto>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var response = new GenericResponse<TagDto>() { Success = false, StatusCode = 400 };
            try
            {
                if (string.IsNullOrWhiteSpace(request.TagName))
                {
                    response.Message = "Tag name cannot be empty";
                    return response;
                }

                var currentUser = await _userContext.GetCurrentUser();
                bool isEditable = currentUser != null && currentUser.Roles.Contains("Admin");
                if (!isEditable)
                {
                    response.Message = "User is not authorized to access";
                    response.StatusCode = 401;
                    return response;
                }

                var tagFromDb = await _db.Tags
                .FirstOrDefaultAsync(x => x.Value.ToLower() == request.TagName.ToLower());
                if (tagFromDb != null)
                {
                    response.Message = "Tag already exists";
                    response.StatusCode = 409;
                    return response;
                }

                var tag = new Tag()
                {
                    Value = request.TagName,
                };

                _db.Tags.Add(tag);
                await _db.SaveChangesAsync();

                response.Success = true;
                response.StatusCode = 201;
                response.Data = _mapper.Map<TagDto>(tag);
                response.Message = $"Tag added succesfully. Resource can be found at /api/v1/tag/{tag.Id}";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;

        }
    }
}
