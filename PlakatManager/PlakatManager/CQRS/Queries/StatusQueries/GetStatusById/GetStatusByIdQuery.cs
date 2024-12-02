using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using MediatR;

namespace ElectionMaterialManager.CQRS.Queries.StatusQueries.GetStatusById
{
    public class GetStatusByIdQuery: IRequest<GenericResponse<StatusDto>>
    {
        public int Id { get; set; }
    }
}
