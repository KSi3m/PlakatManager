using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemById
{
    public class GetElectionItemByIdQuery: IRequest<GenericResponse<ElectionItemDto>>
    {
        public int Id { get; set; } 
        public bool Detailed { get; set; }
    }
}
