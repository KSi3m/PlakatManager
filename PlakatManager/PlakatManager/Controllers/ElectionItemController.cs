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

namespace ElectionMaterialManager.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class ElectionItemController : ControllerBase
    {
        private readonly ElectionMaterialManagerContext _db;
        private readonly IMediator _mediator;

       public ElectionItemController(ElectionMaterialManagerContext db, IMediator mediator) {
            _db = db;
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
            return BadRequest();
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
            return BadRequest();
        
        }
        [HttpDelete]
        [Route("election-item/{id}")]
        public async Task<IActionResult> DeleteElectionItem(int id)
        {
            var response = await _mediator.Send(new DeleteElectionItemCommand() { Id = id });
            if(response.Success)
                return NoContent();
            return BadRequest();
        }

        [HttpPatch]
        [Route("election-item/{id}")]
        public async Task<IActionResult> UpdateElectionItem(EditElectionItemCommand command, int id)
        {
         
            command.Id = id;
            var response = await _mediator.Send(command);
            if(response.Success)
                return NoContent();
            return BadRequest();

        }

        [HttpPost]
        [Route("election-item/led")]
        public async Task<IActionResult> CreateLed(LEDRequestDTO dto)
        {
            var led = new LED
            {
                Area = dto.Area,

                Latitude = (double)dto.Latitude,
                Longitude = (double)dto.Longitude,
                Priority = (int)dto.Priority,
                Size = dto.Size,
                Cost = (decimal)dto.Cost,
                StatusId = dto.StatusId,
                AuthorId = dto.AuthorId,
                RefreshRate = (int)dto.RefreshRate,
                Resolution = dto.Resolution,
            };
            _db.Add(led);
            await _db.SaveChangesAsync();

            return Created($"/api/v1/election-item/{led.Id}", led);

        }

        [HttpPost]
        [Route("election-item/poster")]
        public async Task<IActionResult> CreatePoster(PosterRequestDTO dto)
        {
            var poster = new Poster
            {
                Area = dto.Area,
                Latitude = (double)dto.Latitude,
                Longitude = (double)dto.Longitude,
                Priority = (int)dto.Priority,
                Size = dto.Size,
                Cost = (decimal)dto.Cost,
                StatusId = dto.StatusId,
                AuthorId = dto.AuthorId,
                PaperType = dto.PaperType

            };
            _db.Add(poster);
            await _db.SaveChangesAsync();

            return Created($"/api/v1/election-item/{poster.Id}", poster);

        }

        [HttpPost]
        [Route("election-item/billboard")]
        public async Task<IActionResult> CreateBillboard(BillboardRequestDTO dto)
        {
            var billboard = new Billboard
            {
                Area = dto.Area,
                Latitude = (double)dto.Latitude,
                Longitude = (double)dto.Longitude,
                Priority = (int)dto.Priority,
                Size = dto.Size,
                Cost = (decimal)dto.Cost,
                StatusId = dto.StatusId,
                AuthorId = dto.AuthorId,
                StartDate = (DateTime)dto.StartDate,
                EndDate = (DateTime)dto.EndDate
            };
            _db.Add(billboard);
            await _db.SaveChangesAsync();

            return Created($"/api/v1/election-item/{billboard.Id}", billboard);

        }

        [HttpPost]
        [Route("election-item")]
        public async Task<IActionResult> CreateElectionItem(ElectionItemRequestDTO dto,  ElectionItemFactoryRegistry factoryRegistry)
        {
            var type = dto.Type;
            var electionItem = factoryRegistry.CreateElectionItem(type, dto);

            _db.Add(electionItem);
            await _db.SaveChangesAsync();

            return Created($"/api/v1/election-item/{electionItem.Id}", electionItem); 

        }

        [HttpGet]
        [Route("/election-items-by-tag")]
        public async Task<IActionResult> GetElectionItemsByTag(string tag)
        {
            if (tag.IsNullOrEmpty()) return BadRequest();

            var query = new GetElectionItemsByTagQuery() { TagName = tag };
            var response = await _mediator.Send(query);

            if(response.Success)
                return Ok(response);
            return BadRequest(response.Message);
        }



    }
}
