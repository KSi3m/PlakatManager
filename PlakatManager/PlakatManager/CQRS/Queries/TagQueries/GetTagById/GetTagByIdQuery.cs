using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;

namespace ElectionMaterialManager.CQRS.Queries.TagQueries.GetTagById
{
    public class GetTagByIdQuery: IRequest<GenericResponse<TagDto>>
    {
        public int Id { get; set; } 
    }
}
