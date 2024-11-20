using ElectionMaterialManager.Entities;
using MediatR;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemById
{
    public class GetElectionItemByIdQuery: IRequest<GenericResponse<ElectionItem>>
    {
        public int Id { get; set; } 
    }
}
