using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using MediatR;

namespace ElectionMaterialManager.CQRS.Queries.UserQueries.GetUserComments
{
    public class GetUserCommentsQuery: IRequest<GenericResponseWithList<UserCommentDto>>
    {
    }
}
