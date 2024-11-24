using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;

namespace ElectionMaterialManager.CQRS.Queries.TagQueries.GetAllTags
{
    public class GetAllTagsQuery: IRequest<GenericResponseWithList<TagDto>>
    {
    }
}
