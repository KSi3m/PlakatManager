using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Entities;
using MediatR;

namespace ElectionMaterialManager.CQRS.Queries.TagQueries.GetTagById
{
    public class GetTagByIdQuery: IRequest<GenericResponse<Tag>>
    {
        public int Id { get; set; } 
    }
}
