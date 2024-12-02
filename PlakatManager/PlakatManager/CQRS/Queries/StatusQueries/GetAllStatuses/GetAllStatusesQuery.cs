using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;

namespace ElectionMaterialManager.CQRS.Queries.StatusQueries.GetAllStatuses
{
    public class GetAllStatusesQuery : IRequest<GenericResponseWithList<StatusDto>>
    {
    }
}
