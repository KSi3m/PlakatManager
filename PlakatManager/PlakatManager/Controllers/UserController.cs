using ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItems;
using ElectionMaterialManager.CQRS.Queries.UserQueries.GetUsersElectionItems;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectionMaterialManager.Controllers
{

    [Route("api/v1/")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        [Route("user/election-items")]
        public async Task<IActionResult> GetMyElectionItems()
        {

            var query = new GetUsersElectionItemsQuery();
            var response = await _mediator.Send(query);
            if (response.Success)
                return Ok(response);
            return BadRequest(new { response.Message });
        }
    }
}
