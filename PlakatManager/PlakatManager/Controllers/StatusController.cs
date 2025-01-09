using Azure.Core;
using ElectionMaterialManager.CQRS.Commands.StatusCommands.CreateStatus;
using ElectionMaterialManager.CQRS.Commands.StatusCommands.DeleteStatus;
using ElectionMaterialManager.CQRS.Commands.StatusCommands.UpdateStatus;
using ElectionMaterialManager.CQRS.Commands.TagCommands.CreateTag;
using ElectionMaterialManager.CQRS.Commands.TagCommands.DeleteTag;
using ElectionMaterialManager.CQRS.Commands.TagCommands.UpdateTag;
using ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemById;
using ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItems;
using ElectionMaterialManager.CQRS.Queries.StatusQueries.GetAllStatuses;
using ElectionMaterialManager.CQRS.Queries.StatusQueries.GetStatusById;
using ElectionMaterialManager.CQRS.Queries.TagQueries.GetAllTags;
using ElectionMaterialManager.CQRS.Queries.TagQueries.GetTagById;
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
    public class StatusController: ControllerBase
    {
        private readonly IMediator _mediator;

        public StatusController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SwaggerOperation(Summary = "Get all statuses",
        Description = "This endpoint retrieves a list of all possible statuses that can be assigned to election items.")]
        [ProducesResponseType(200)] 
        [ProducesResponseType(400)]
        [HttpGet]
        [Route("statuses")]
        public async Task<IActionResult> GetAllStatuses()
        {
            var response = await _mediator.Send(new GetAllStatusesQuery());
            if (response.Success)
            {
                return Ok(new { response.StatusCode, response.Message, response.Data });
            }
            return StatusCode(response.StatusCode, new { response.StatusCode, response.Message });
        }

        [SwaggerOperation(Summary = "Get status by ID",
         Description = "This endpoint retrieves the details of a specific status by its unique identifier.")]
        [ProducesResponseType(200)] 
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [HttpGet]
        [Route("status/{id}")]
        public async Task<IActionResult> GetStatus([FromRoute] GetStatusByIdQuery query, int id)
        {
            var response = await _mediator.Send(query);
            if (response.Success)
            {
                return Ok(new { response.StatusCode, response.Message, response.Data });
            }
            return StatusCode(response.StatusCode, new { response.StatusCode, response.Message });
        }


        [SwaggerOperation(Summary = "Create a new status",
        Description = "This endpoint allows authorized users to create a new status for election items. " +
                  "The status name must be unique and non-empty.")]
        [ProducesResponseType(201)] 
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpPost]
        [Authorize]
        [Route("status")]
        public async Task<IActionResult> CreateStatus(CreateStatusCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Success)
                return StatusCode(201, new { response.StatusCode, response.Message });
            return StatusCode(response.StatusCode, new { response.StatusCode, response.Message });

        }

        [SwaggerOperation(Summary = "Update an existing status",
        Description = "This endpoint allows authorized users to update the name of an existing status. " +
                  "The new name must be unique and non-empty.")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)] 
        [ProducesResponseType(401)] 
        [HttpPut]
        [Authorize]
        [Route("status/{id}")]
        public async Task<IActionResult> UpdateStatus(UpdateStatusCommand command, int id)
        {
            if (id <= 0) return StatusCode(400,new { StatusCode = 400, Message = "Wrong Id supplied" });
            command.Id = id;
            var response = await _mediator.Send(command);
            if (response.Success)
                return StatusCode(204, new { response.StatusCode, response.Message });
            return StatusCode(response.StatusCode, new { response.StatusCode, response.Message });

        }

        [SwaggerOperation(Summary = "Delete a status by ID",
        Description = "This endpoint allows authorized users to delete a status by its unique ID. ")]
        [ProducesResponseType(204)] 
        [ProducesResponseType(400)] 
        [HttpDelete]
        [Authorize]
        [Route("status/{id}")]
        //konflikty
        public async Task<IActionResult> DeleteStatus([FromRoute] DeleteStatusCommand command, int id)
        {
       
            var response = await _mediator.Send(command);
            if (response.Success)
                return StatusCode(204, new { response.StatusCode, response.Message });
            return StatusCode(response.StatusCode, new { response.StatusCode, response.Message });
        }


    }
}
