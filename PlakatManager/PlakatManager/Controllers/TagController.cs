using Azure.Core;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.Controllers
{

    [Route("api/v1/")]
    [ApiController]
    public class TagController: ControllerBase
    {
        private readonly ElectionMaterialManagerContext _db;

        public TagController(ElectionMaterialManagerContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("tags")]
        public async Task<IActionResult> GetAllTags()
        {
            var tags = await _db.Tags.ToListAsync();

            return Ok(tags);
        }

        [HttpGet]
        [Route("tag/{id}")]
        public async Task<IActionResult> GetTag(int id)
        {
            var tag = await _db.Tags.FirstOrDefaultAsync(x => x.Id == id);

            return Ok(tag);
        }
        [HttpPost]
        [Route("tag")]
        public async Task<IActionResult> CreateTag(TagRequestDTO request)
        {
            var tagFromDb = await _db.Tags
                .FirstOrDefaultAsync(x => x.Value.ToLower() == request.Value.ToLower());
            if (tagFromDb != null) return Conflict(new { message = "Tag already exists" });

            var tag = new Tag()
            {
                Value = request.Value,
            };

            _db.Tags.Add(tag);
            await _db.SaveChangesAsync();

            return Created($"/api/v1/tag/{tag.Id}", tag);
        }

        [HttpPatch]
        [Route("tag/{id}")]
        public async Task<IActionResult> UpdateTag(TagRequestDTO request)
        {
            var tagFromDb = await _db.Tags
                .FirstOrDefaultAsync(x => x.Value.ToLower() == request.Value.ToLower());
            if (tagFromDb != null) return Conflict(new { message = "Tag already exists" });

            var tag = new Tag()
            {
                Value = request.Value,
            };

            _db.Tags.Add(tag);
            await _db.SaveChangesAsync();

            return Created($"/api/v1/tag/{tag.Id}", tag);
        }

    }
}
