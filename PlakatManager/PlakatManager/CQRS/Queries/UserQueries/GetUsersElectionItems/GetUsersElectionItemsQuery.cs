using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using MediatR;

namespace ElectionMaterialManager.CQRS.Queries.UserQueries.GetUsersElectionItems
{
    public class GetUsersElectionItemsQuery: IRequest<GenericResponseWithList<ElectionItemDto>>
    {

    }
}
