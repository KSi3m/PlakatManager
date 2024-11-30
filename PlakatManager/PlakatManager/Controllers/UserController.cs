using ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItems;
using ElectionMaterialManager.CQRS.Queries.UserQueries.GetUserComments;
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
                return Ok(new { MyElectionItems = response.Data });
            return BadRequest(new { response.Message });
        }

        [HttpGet]
        [Authorize]
        [Route("user/comments")]
        public async Task<IActionResult> GetMyComments()
        {
            var query = new GetUserCommentsQuery();
            var response = await _mediator.Send(query);
            if (response.Success)
                return Ok(new { MyComments = response.Data });
            return BadRequest(new { response.Message });
        }
    }
}
