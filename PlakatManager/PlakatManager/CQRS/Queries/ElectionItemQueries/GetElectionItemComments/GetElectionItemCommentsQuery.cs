using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemComments
{
    public class GetElectionItemCommentsQuery: IRequest<GenericResponseWithList<CommentDto>>
    {
        public int Id { get; set; }

    }
}
