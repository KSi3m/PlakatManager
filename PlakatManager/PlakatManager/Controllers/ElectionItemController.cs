using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using ElectionMaterialManager.Utilities;
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

       public ElectionItemController(ElectionMaterialManagerContext db) {
            _db = db;
       }

        [HttpGet]
        [Route("election-items")]
        public async Task<IActionResult> GetElectionItems(int indexRangeStart = 1, int indexRangeEnd = 10)
        {
            var electionItems = await _db.ElectionItems
                .Where(x => x.Id >= indexRangeStart && x.Id <= indexRangeEnd)
                .ToListAsync();

            if (electionItems == null)
            {
                var errorResponse = new
                {
                    status = 404,
                    message = $"Election items withing given range not found.",
                    details = "The requested items does not exist in the database."
                };
                return NotFound(errorResponse);
            }

            return Ok(electionItems);

        }

        [HttpGet]
        [Route("election-item/{id}")]
        public async Task<IActionResult> GetElectionItem(int id)
        {
            var electionItem = await _db.ElectionItems.FirstOrDefaultAsync(x => x.Id == id);

            if (electionItem == null)
            {
                var errorResponse = new
                {
                    status = 404,
                    message = $"Election item with id {id} not found.",
                    details = "The requested item does not exist in the database."
                };
                return NotFound(errorResponse);
            }

            return Ok(electionItem);
        }
        [HttpDelete]
        [Route("election-item/{id}")]
        public async Task<IActionResult> DeleteElectionItem(int id)
        {
            var electionItem = await _db.ElectionItems.FirstAsync(x => x.Id.Equals(id));

            if (electionItem == null)
            {
                var errorResponse = new
                {
                    status = 404,
                    message = $"Election item with id {id} not found.",
                    details = "The requested item does not exist in the database."
                };
                return NotFound(errorResponse);
            }

            _db.Remove(electionItem);
            await _db.SaveChangesAsync();

            return NoContent();


        }

        [HttpPatch]
        [Route("election-item/{id}")]
        public async Task<IActionResult> UpdateElectionItem(int id)
        {
            var electionItem = await _db.ElectionItems.FirstAsync(x => x.Equals(id));

            if (electionItem == null)
            {
                var errorResponse = new
                {
                    status = 404,
                    message = $"Election item with id {id} not found.",
                    details = "The requested item does not exist in the database."
                };
                return NotFound(errorResponse);
            }

            _db.Remove(electionItem);
            await _db.SaveChangesAsync();

            return NoContent();

        }

        [HttpPost]
        [Route("election-item/led")]
        public async Task<IActionResult> CreateLed(LEDRequestDTO dto,int id)
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
        public async Task<IActionResult> CreatePoster(PosterRequestDTO dto, int id)
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
        public async Task<IActionResult> CreateBillboard(BillboardRequestDTO dto, int id)
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
        public async Task<IActionResult> CreateElectionItem(ElectionItemRequestDTO dto, int id, ElectionItemFactoryRegistry factoryRegistry)
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

            var electionItems = await _db.ElectionItems
            .Include(x => x.Tags)
            .Where(x => x.Tags.Any(y => y.Value == tag))
            .ToListAsync();

            // if (electionItems.Count == 0) 

            return Ok(electionItems);
        }



    }
}
