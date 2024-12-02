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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        [Route("statuses")]
        public async Task<IActionResult> GetAllStatuses()
        {
            var response = await _mediator.Send(new GetAllStatusesQuery());
            if (response.Success)
                return Ok(response.Data);
            return BadRequest(new { response.Message });
        }

        [HttpGet]
        [Route("status/{id}")]
        public async Task<IActionResult> GetStatus([FromRoute] GetStatusByIdQuery query, int id)
        {
            var response = await _mediator.Send(query);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return BadRequest(new { response.Message });
        }

     

        [HttpPost]
        [Route("status")]
        public async Task<IActionResult> CreateStatus(CreateStatusCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Success)
                return Created(response.Message, response.Data);
            return BadRequest(new { response.Message });

        }

        [HttpPut]
        [Route("status/{id}")]
        public async Task<IActionResult> UpdateStatus(UpdateStatusCommand command, int id)
        {
            command.Id = id;
            var response = await _mediator.Send(command);
            if (response.Success)
                return NoContent();
            return BadRequest(new { response.Message });

        }

        [HttpDelete]
        [Route("status/{id}")]
        public async Task<IActionResult> DeleteStatus([FromRoute] DeleteStatusCommand command, int id)
        {
            //var command = new DeleteStatusCommand() { Id = id };
            var response = await _mediator.Send(command);
            if (response.Success)
            {
                return NoContent();
            }
            return BadRequest(new { response.Message });
        }


    }
}
