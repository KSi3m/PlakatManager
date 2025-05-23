﻿using Azure.Core;
using ElectionMaterialManager.CQRS.Commands.TagCommands.CreateTag;
using ElectionMaterialManager.CQRS.Commands.TagCommands.DeleteTag;
using ElectionMaterialManager.CQRS.Commands.TagCommands.UpdateTag;
using ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemById;
using ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItems;
using ElectionMaterialManager.CQRS.Queries.TagQueries.GetAllTags;
using ElectionMaterialManager.CQRS.Queries.TagQueries.GetTagById;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ElectionMaterialManager.Controllers
{

    [Route("api/v1/")]
    [ApiController]
    public class TagController: ControllerBase
    {
        private readonly IMediator _mediator;

        public TagController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SwaggerOperation(Summary = "Get all tags",
        Description = "This endpoint retrieves all available tags from the system. ")]
        [ProducesResponseType(typeof(GenericResponseWithList<TagDto>),200)]
        [ProducesResponseType(typeof(GenericResponseWithList<TagDto>),400)]
        [ProducesResponseType(typeof(GenericResponseWithList<TagDto>),404)] 
        [HttpGet]
        [Route("tags")]
        public async Task<IActionResult> GetAllTags()
        {
            var response = await _mediator.Send(new GetAllTagsQuery());
            if (response.Success)
                return Ok(response);
            return StatusCode(response.StatusCode, response);
        }

        [SwaggerOperation(Summary = "Get a tag by ID",
        Description = "This endpoint retrieves a specific tag from the system by its unique ID.")]
        [ProducesResponseType(typeof(GenericResponse<TagDto>),200)] 
        [ProducesResponseType(typeof(GenericResponse<TagDto>),400)]
        [ProducesResponseType(typeof(GenericResponse<TagDto>),404)] 
        [HttpGet]
        [Route("tag/{id}")]
        public async Task<IActionResult> GetTag([FromRoute] GetTagByIdQuery query, int id)
        {
            var response = await _mediator.Send(query);
            if (response.Success)
            {
                return Ok(response);
            }
            return StatusCode(response.StatusCode, response);
 
        }

        [SwaggerOperation(Summary = "Delete a tag by ID",
        Description = "This endpoint allows authorized users to delete a tag by its unique ID. ")]
        [ProducesResponseType(typeof(Response),204)] 
        [ProducesResponseType(typeof(Response),400)]
        [ProducesResponseType(typeof(Response),401)]
        [ProducesResponseType(typeof(Response),404)] 
        //[ProducesResponseType(typeof(Response),409)] 
        [HttpDelete]
        //[Authorize]
        [Route("tag/{id}")]
        public async Task<IActionResult> DeleteTag([FromRoute] DeleteTagCommand command, int id)
        {
        
            var response = await _mediator.Send(command);
            if (response.Success)
            {
                return StatusCode(204, response);
            }
            return StatusCode(response.StatusCode, response);
        }
        

        [SwaggerOperation(Summary = "Create a new tag",
        Description = "This endpoint allows authorized users to create a new tag in the system. ")]
        [ProducesResponseType(typeof(Response),201)]
        [ProducesResponseType(typeof(Response),400)]
        [ProducesResponseType(typeof(Response),401)]
        [ProducesResponseType(typeof(Response),409)]
        [HttpPost]
       // [Authorize]
        [Route("tag")]
        public async Task<IActionResult> CreateTag(CreateTagCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Success)
                return StatusCode(201, response);
            return StatusCode(response.StatusCode, response);

        }

        [SwaggerOperation(Summary = "Update an existing tag",
        Description = "This endpoint allows updating an existing tag by its ID. Only authorized users can update tags.")]
        [ProducesResponseType(typeof(Response),204)] 
        [ProducesResponseType(typeof(Response),400)]
        [ProducesResponseType(typeof(Response),401)]
        [ProducesResponseType(typeof(Response),404)]
        [ProducesResponseType(typeof(Response),409)]
        [HttpPut]
       // [Authorize]
        [Route("tag/{id}")]
        public async Task<IActionResult> UpdateTag(UpdateTagCommand command, int id)
        {
            if (id <= 0) return StatusCode(400, new Response
            {
                Success = false,
                StatusCode = 400,
                Message = "One or more validation errors has occurred",
                Errors = new List<string>() { "Wrong Id supplied" }
            }); 
            command.Id = id;
            var response = await _mediator.Send(command);
            if (response.Success)
                return StatusCode(204, response);
            return StatusCode(response.StatusCode, response);

        }

    }
}
