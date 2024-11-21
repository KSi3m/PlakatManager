using Azure.Core;
using ElectionMaterialManager.CQRS.Commands.TagCommands.CreateTag;
using ElectionMaterialManager.CQRS.Commands.TagCommands.DeleteTag;
using ElectionMaterialManager.CQRS.Commands.TagCommands.UpdateTag;
using ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemById;
using ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItems;
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
    public class TagController: ControllerBase
    {
        private readonly IMediator _mediator;

        public TagController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("tags")]
        public async Task<IActionResult> GetAllTags()
        {
            var response = await _mediator.Send(new GetAllTagsQuery());
            if (response.Success)
                return Ok(response);
            return BadRequest(new { response.Message });
        }

        [HttpGet]
        [Route("tag/{id}")]
        public async Task<IActionResult> GetTag(int id)
        {
            var query = new GetTagByIdQuery() { Id = id };
            var response = await _mediator.Send(query);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(new { response.Message });
        }

        [HttpDelete]
        [Route("tag/{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var command = new DeleteTagCommand() { Id = id };
            var response = await _mediator.Send(command);
            if (response.Success)
            {
                return NoContent();
            }
            return BadRequest(new { response.Message });
        }


        [HttpPost]
        [Route("tag")]
        public async Task<IActionResult> CreateTag(CreateTagCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Success)
                return Created(response.Message, response.Data);
            return BadRequest(new { response.Message });

        }

        [HttpPatch]
        [Route("tag/{id}")]
        public async Task<IActionResult> UpdateTag(UpdateTagCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Success)
                return NoContent();
            return BadRequest(new { response.Message });

        }

    }
}
