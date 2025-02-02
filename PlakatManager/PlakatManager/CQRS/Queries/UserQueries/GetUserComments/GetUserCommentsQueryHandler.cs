﻿using AutoMapper;
using ElectionMaterialManager.AppUserContext;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Queries.UserQueries.GetUserComments
{
    public class GetUserCommentsQueryHandler : IRequestHandler<GetUserCommentsQuery, GenericResponseWithList<UserCommentDto>>
    {
        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public GetUserCommentsQueryHandler(ElectionMaterialManagerContext db, IMapper mapper, IUserContext userContext)
        {
            _db = db;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<GenericResponseWithList<UserCommentDto>> Handle(GetUserCommentsQuery request, CancellationToken cancellationToken)
        {
            var response = new GenericResponseWithList<UserCommentDto>() { Data = [], Success = false , StatusCode = 400 };
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


                var comments = await _db.Comments
                    .Include(x => x.ElectionItem)
                    .Where(x => x.AuthorId == currentUser.Id)
                    .Select(x => new UserCommentDto
                    {
                        ElectionItemId = x.ElectionItemId,
                        Message = x.Message,
                        CreatedAt = x.CreatedAt
                    })
                    .ToListAsync();

                /*if (comments == null)// || !comments.Any())
                {
                    response.Message = "Error";
                    return response;
                }*/
                response.Message = $"Comments by user found ";
                response.Success = true;
                response.StatusCode = 200;
                response.Data = comments;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
