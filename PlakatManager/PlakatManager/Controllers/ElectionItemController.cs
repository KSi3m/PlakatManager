using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateBillboard;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateElectionItem;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateLED;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreatePoster;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.DeleteElectionItem;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.EditElectionItem;
using ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemById;
using ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItems;
using ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemsByTag;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using ElectionMaterialManager.Utilities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ElectionMaterialManager.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class ElectionItemController : ControllerBase
    {
       private readonly IMediator _mediator;

       public ElectionItemController(IMediator mediator) {
            _mediator = mediator;
       }

        [HttpGet]
        [Route("election-items")]
        public async Task<IActionResult> GetElectionItems(int indexRangeStart = 1, int indexRangeEnd = 10)
        {

            var query = new GetElectionItemsQuery()
            { IndexRangeStart = indexRangeStart, IndexRangeEnd = indexRangeEnd };
            var response = await _mediator.Send(query);
            if (response.Success)
                return Ok(response);
            return BadRequest(new { response.Message });
        }

        [HttpGet]
        [Route("election-item/{id}")]
        public async Task<IActionResult> GetElectionItem(int id)
        {
            var query = new GetElectionItemByIdQuery() { Id = id };
            var response = await _mediator.Send(query);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(new { response.Message });
        
        }
        [HttpDelete]
        [Route("election-item/{id}")]
        public async Task<IActionResult> DeleteElectionItem(int id)
        {
            var response = await _mediator.Send(new DeleteElectionItemCommand() { Id = id });
            if(response.Success)
                return NoContent();
            return BadRequest(new { response.Message });
        }

        [HttpPatch]
        [Route("election-item/{id}")]
        public async Task<IActionResult> UpdateElectionItem(EditElectionItemCommand command, int id)
        {
         
            command.Id = id;
            var response = await _mediator.Send(command);
            if(response.Success)
                return NoContent();
            return BadRequest(new { response.Message });

        }

        [HttpPost]
        [Route("election-item/led")]
        public async Task<IActionResult> CreateLed(CreateLEDCommand command)
        {
            var response = await _mediator.Send(command);
            if(response.Success)
                return Created(response.Message,response.Data);
            return BadRequest(new { response.Message });


        }

        [HttpPost]
        [Route("election-item/poster")]
        public async Task<IActionResult> CreatePoster(CreatePosterCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Success)
                return Created(response.Message, response.Data);
            return BadRequest(new { response.Message });

        }

        [HttpPost]
        [Route("election-item/billboard")]
        public async Task<IActionResult> CreateBillboard(CreateBillboardCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Success)
                return Created(response.Message, response.Data);
            return BadRequest(new { response.Message });

        }

        [HttpPost]
        [Route("election-item")]
        public async Task<IActionResult> CreateElectionItem(CreateElectionItemCommand command)
        {

            var response = await _mediator.Send(command);
            if (response.Success)
                return Created(response.Message, response.Data);
            return BadRequest(new { response.Message });

        }

        [HttpGet]
        [Route("election-items-by-tag")]
        public async Task<IActionResult> GetElectionItemsByTag(string tag)
        {
            if (tag.IsNullOrEmpty()) return BadRequest();

            var query = new GetElectionItemsByTagQuery() { TagName = tag };
            var response = await _mediator.Send(query);

            if(response.Success)
                return Ok(new { tag, electionItems = response.Data });
            return BadRequest(new { response.Message });
        }
       



    }
}
