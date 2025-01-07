using ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItems;
using ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetNearbyElectionItems;
using ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetSoonExpiringElectionItems;
using ElectionMaterialManager.CQRS.Queries.UserQueries.GetElectionItemsByPriority;
using ElectionMaterialManager.CQRS.Queries.UserQueries.GetExpiredElectionItems;
using ElectionMaterialManager.CQRS.Queries.UserQueries.GetUserComments;
using ElectionMaterialManager.CQRS.Queries.UserQueries.GetUsersElectionItems;
using ElectionMaterialManager.NWM;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Summary = "Get election items for the authenticated user",
         Description = "This endpoint returns the election items created by the authenticated user. " +
                  "Only authorized users can access this information.")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)] 
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

        [SwaggerOperation(Summary = "Get comments for the authenticated user",
        Description = "This endpoint returns all comments made by the authenticated user on election items. " +
                  "Only authorized users can access this information.")]
        [ProducesResponseType(200)] 
        [ProducesResponseType(401)] 
        [ProducesResponseType(400)] 
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

        [SwaggerOperation(Summary = "Get election items with a priority range for the authenticated user",
       Description = "This endpoint returns the election items for the authenticated user that fall within the specified priority range. " +
                  "The priority range is defined by the minimum and maximum values. Only authorized users can access this information.")]
        [ProducesResponseType(200)] 
        [ProducesResponseType(400)] 
        [ProducesResponseType(401)] 
        [HttpGet]
        [Authorize]
        [Route("user/election-items/{minPriority}-{maxPriority}")]
        public async Task<IActionResult> GetMyElectionItemsWithTopPriority([FromRoute] GetElectionItemsByPriorityQuery query, int minPriority, int maxPriority)
        {
            var response = await _mediator.Send(query);
            if (response.Success)
                return Ok(new { MyElectionItems = response.Data });
            return BadRequest(new { response.Message });
        }

        [SwaggerOperation(Summary = "Get expired election items for the authenticated user",
        Description = "This endpoint returns the election items that have expired for the authenticated user. " +
                  "Only authorized users can access this information.")]
        [ProducesResponseType(200)] 
        [ProducesResponseType(401)] 
        [ProducesResponseType(400)]
        [HttpGet]
        [Route("user/election-items/expired")]
        public async Task<IActionResult> GetExpiredElectionItems([FromRoute] GetExpiredElectionItemsQuery query)
        {
            query.UserOnly = true;
            var response = await _mediator.Send(query);
            if (response.Success)
                return Ok(response);
            return BadRequest(new { response.Message });
        }

        [SwaggerOperation(Summary = "Get election items expiring soon for the authenticated user",
        Description = "This endpoint returns the election items that will expire within the specified number of days for the authenticated user. " +
                  "Only authorized users can access this information.")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)] 
        [ProducesResponseType(401)] 
        [HttpGet]
        [Route("user/election-items/expiring-soon/{days}")]
        public async Task<IActionResult> GetSoonExpiringElectionItems([FromRoute] GetSoonExpiringElectionItemsQuery query, int days)
        {

            query.UserOnly = true;
            var response = await _mediator.Send(query);

            if (response.Success)
                return Ok(response);
            return BadRequest(new { response.Message });
        }



        /* [HttpGet]
         [Route("test/{longitude}-{latitude}")]
         public async Task<IActionResult> Testing(double longitude, double latitude)
         {

             var helper = new Helper(longitude, latitude);

             if(helper.help(out string name))
             {
                 return Ok(new { District = name } );
             }
             return Ok( new {Message = "District not found"});

         }*/


    }
}
