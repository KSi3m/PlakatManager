using AutoMapper;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemComments
{
    public class GetElectionItemCommentsQueryHandler : IRequestHandler<GetElectionItemCommentsQuery, GenericResponseWithList<CommentDto>>
    {

        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;

        public GetElectionItemCommentsQueryHandler(ElectionMaterialManagerContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<GenericResponseWithList<CommentDto>> Handle(GetElectionItemCommentsQuery request, CancellationToken cancellationToken)
        {
            var response = new GenericResponseWithList<CommentDto>() { Data = [], Success = false };
            try
            {
                var comments = await _db.Comments
                    .Include(x => x.ElectionItem)
                    .Include(x=>x.Author)
                    .Where(x => x.ElectionItem.Id == request.Id)
                    .Select(x => new CommentDto{
                        Message = x.Message, 
                        Author = new AuthorDto{
                            Id = x.AuthorId,
                            FirstName = x.Author.FirstName,
                            LastName = x.Author.LastName
                        },
                        CreatedAt = x.CreatedAt
                    })
                    .ToListAsync();

                if (comments == null)// || !comments.Any())
                {
                    response.Message = "Error";
                    return response;
                }
                response.Message = $"Comments for  elcetion item with id {request.Id} found ";
                response.Success = true;

                //response.Data = _mapper.Map<IEnumerable<CommentDto>>(comments);
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
