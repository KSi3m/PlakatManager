using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemById
{
    public class GetElectionItemByIdQuery: IRequest<GenericResponse<ElectionItemDto>>
    {

        public int Id { get; set; }
  
    }
}
