using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemByIdDetail
{
    public class GetElectionItemByIdDetailQuery : IRequest<GenericResponse<ElectionItemDetailDto>>
    { 
        public int Id { get; set; }
    }
}
