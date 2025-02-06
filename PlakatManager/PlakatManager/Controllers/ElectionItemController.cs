using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateBillboard;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateElectionItem;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateLED;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreatePoster;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.DeleteElectionItem;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.UpdateElectionItemPartially;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.UpdateElectionItemFully;
using ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemById;
using ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemComments;
using ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItems;
using ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemsByTag;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using ElectionMaterialManager.Utilities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.AddCommentToElectionItem;
using ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemsByDistrict;
using ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetNearbyElectionItems;
using ElectionMaterialManager.CQRS.Queries.UserQueries.GetExpiredElectionItems;
using ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetSoonExpiringElectionItems;
using ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemByIdDetail;
using Swashbuckle.AspNetCore.Annotations;
using ElectionMaterialManager.CQRS.Responses;


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

        [SwaggerOperation(Summary = "Retrieve all Election Items",
            Description = "This operation retrieves a list of all election items available in the system." +
            "You can specify range in parameters. If no items are found, an empty list is returned.")]
        [ProducesResponseType(typeof(GenericResponseWithList<ElectionItemDto>), 200)]
        [ProducesResponseType(typeof(GenericResponseWithList<ElectionItemDto>), 400)]
        [HttpGet]
        [Route("election-items")]
        public async Task<IActionResult> GetElectionItems([FromQuery] GetElectionItemsQuery query)
        {
            var response = await _mediator.Send(query);
            if (response.Success)
                return StatusCode(200, response);
            return StatusCode(response.StatusCode, response);
        }


        [SwaggerOperation(Summary = "Retrieve nearby election items",
        Description = "This endpoint retrieves a list of election items that are within a specified radius (in kilometers) from a given latitude and longitude. " 
        +"If no items are found, an empty list is returned.")]
        [ProducesResponseType(typeof(GenericResponseWithList<ElectionItemDto>), 200)]
        [ProducesResponseType(typeof(GenericResponseWithList<ElectionItemDto>), 400)]
        [HttpGet]
        [Route("election-items/nearby{latitude}-{longitude}-{radiusInKM}")]
        public async Task<IActionResult> GetNearbyElectionItems([FromRoute] GetNearbyElectionItemsQuery query, double longitude, double latitude, double radiusInKM)
        {
            var response = await _mediator.Send(query);
            if (response.Success)
                return StatusCode(200, response);
            return StatusCode(response.StatusCode, response);
        }

        [SwaggerOperation(Summary = "Retrieve one Election Items",
           Description = "This operation retrieves a election item with specified id")]
        [ProducesResponseType(typeof(GenericResponseWithList<ElectionItemDetailDto>), 200)]
        [ProducesResponseType(typeof(GenericResponseWithList<ElectionItemDetailDto>), 400)]
        [HttpGet]
        [Route("election-item/{id}")]
        public async Task<IActionResult> GetElectionItem([FromRoute] GetElectionItemByIdQuery query, int id)
        {

            var response = await _mediator.Send(query);
            if (response.Success)
                return StatusCode(200, response);
            return StatusCode(response.StatusCode, response);

        }


        [SwaggerOperation(Summary = "Retrieve one detailed Election Items",
           Description = "This operation retrieves a more detailed election item by specified id. " )]
        [ProducesResponseType(typeof(GenericResponseWithList<ElectionItemDetailDto>), 200)]
        [ProducesResponseType(typeof(GenericResponseWithList<ElectionItemDetailDto>), 400)]
        [HttpGet]
        [Route("election-item/{id}/detail")]
        public async Task<IActionResult> GetElectionItemWithDetails([FromRoute] GetElectionItemByIdDetailQuery query,  int id)
        {
            var response = await _mediator.Send(query);
            if (response.Success)
                return StatusCode(200, response);
            return StatusCode(response.StatusCode, response);

        }
        [SwaggerOperation(Summary = "Delete an election item",
        Description = "This endpoint allows authorized users to delete a specific election item by its id. " +
                  "Only users with proper permissions can perform this action.")]
        [ProducesResponseType(typeof(Response), 204)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 401)]
        [ProducesResponseType(typeof(Response), 404)]
        [HttpDelete]
        //[Authorize]
        [Route("election-item/{id}")]
        public async Task<IActionResult> DeleteElectionItem([FromRoute] DeleteElectionItemCommand command, int id)
        {
            var response = await _mediator.Send(command);
            if (response.Success)
                return StatusCode(204, response);
            return StatusCode(response.StatusCode, response);
        }

        [SwaggerOperation(Summary = "Partially update an election item",
        Description = "This endpoint allows authorized users (that election item belongs to) and admins to update specific fields of an election item identified by its id " +
                  "The request body should contain only the fields that need to be updated")]
        [ProducesResponseType(typeof(Response), 204)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 401)]
        [ProducesResponseType(typeof(Response), 404)]
        [HttpPatch]
        //[Authorize]
        [Route("election-item/{id}")]
        public async Task<IActionResult> UpdateElectionItemPartially(UpdateElectionItemPartiallyCommand command, int id)
        {
            if (id <= 0) return StatusCode(400, new Response
            {
                Success = false,
                StatusCode = 400,
                Message = "One or more validation errors has occurred",
                Errors = new List<string>() { "Wrong Id of comment supplied" }
            });
            command.Id = id;
            var response = await _mediator.Send(command);
            if (response.Success)
                return StatusCode(204, response);
            return StatusCode(response.StatusCode, response);

        }
        [SwaggerOperation(Summary = "Fully update an election item",
         Description = "This endpoint allows authorized users (that election item belongs to) and admins to replace all fields of an election item identified by its id " +
                  "The request body must contain all the required fields to ensure a complete replacement of the resource.")]
        [ProducesResponseType(typeof(Response),204)] 
        [ProducesResponseType(typeof(Response),400)] 
        [ProducesResponseType(typeof(Response),401)] 
        [ProducesResponseType(typeof(Response),404)] 
        [HttpPut]
        //[Authorize]
        [Route("election-item/{id}")]
        public async Task<IActionResult> UpdateElectionItemFully(UpdateElectionItemFullyCommand command, int id)
        {
            if (id <= 0) return StatusCode(400, new Response
            {
                Success = false,
                StatusCode = 400,
                Message = "One or more validation errors has occurred",
                Errors = new List<string>() { "Wrong Id of comment supplied" }
            });
            command.Id = id;
            var response = await _mediator.Send(command);
            if (response.Success)
                return StatusCode(204, response);
            return StatusCode(response.StatusCode, response);
        }

        [SwaggerOperation(Summary = "Create a new LED election item",
         Description = "This endpoint allows authorized users to create a new LED election item. " +
                  "The request body must contain all required fields for the LED item.")]
        [ProducesResponseType(typeof(Response), 201)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 401)]
        [HttpPost]
        //[Authorize]
        [Route("election-item/led")]
        public async Task<IActionResult> CreateLed(CreateLEDCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Success)
                return StatusCode(201, response);
            return StatusCode(response.StatusCode, response);


        }
        [SwaggerOperation(Summary = "Create a new Poster election item",
         Description = "This endpoint allows authorized users to create a new Poster election item. " +
                  "The request body must contain all required fields for the Poster item.")]
        [ProducesResponseType(typeof(Response), 201)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 401)]
        [HttpPost]
        //[Authorize]
        [Route("election-item/poster")]
        public async Task<IActionResult> CreatePoster(CreatePosterCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Success)
                return StatusCode(201, response);
            return StatusCode(response.StatusCode, response);

        }
        [SwaggerOperation(Summary = "Create a new Billboard election item",
        Description = "This endpoint allows authorized users to create a new Billboard election item. " +
              "The request body must contain all required fields for the Billboard item.")]
        [ProducesResponseType(typeof(Response), 201)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 401)]
        [HttpPost]
        //[Authorize]
        [Route("election-item/billboard")]
        public async Task<IActionResult> CreateBillboard(CreateBillboardCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Success)
                return StatusCode(201, response);
            return StatusCode(response.StatusCode, response);

        }

        [SwaggerOperation(Summary = "Create a new election item",
        Description = "This endpoint allows authorized users to create a new election item. The type of election item" +
            "is specified in the body " +
            "The request body must contain all required fields for the item.")]
        [ProducesResponseType(typeof(Response),201)]
        [ProducesResponseType(typeof(Response),400)]
        [ProducesResponseType(typeof(Response),401)]
        [HttpPost]
       // [Authorize]
        [Route("election-item")]
        public async Task<IActionResult> CreateElectionItem(CreateElectionItemCommand command)
        {

            var response = await _mediator.Send(command);
            if (response.Success)
                return StatusCode(201, response);
            return StatusCode(response.StatusCode, response);

        }

        [SwaggerOperation(Summary = "Retrieve election items by tag",
        Description = "This endpoint retrieves a list of election items that match a specific tag. " +
                  "The tag is provided as a query parameter and is used to filter the results. " +
                  "If no items match the provided tag, an empty list is returned.")]
        [ProducesResponseType(typeof(GenericResponseWithList<ElectionItemDto>),200)] 
        [ProducesResponseType(typeof(GenericResponseWithList<ElectionItemDto>),400)] 
       // [ProducesResponseType(typeof(GenericResponseWithList<ElectionItemDto>),404)] 
        [HttpGet]
        [Route("election-items-by-tag/{tagName}")]
        public async Task<IActionResult> GetElectionItemsByTag([FromRoute] GetElectionItemsByTagQuery query, string tagName)
        {
           // if (tag.IsNullOrEmpty()) return BadRequest();

            //var query = new GetElectionItemsByTagQuery() { TagName = tag };
            var response = await _mediator.Send(query);

            if (response.Success)
                return StatusCode(200, response);
            return StatusCode(response.StatusCode, response);
        }




        [SwaggerOperation(Summary = "Retrieve comments for a specific election item",
         Description = "This endpoint retrieves a list of comments associated with a specific election item. " +
                  "The election item's unique ID is provided as a path parameter.")]
        [ProducesResponseType(typeof(GenericResponseWithList<CommentDto>), 200)]
        [ProducesResponseType(typeof(GenericResponseWithList<CommentDto>), 400)]
       // [ProducesResponseType(typeof(Response), 404)]
        [HttpGet]
        [Route("election-item/{id}/comments")]
        public async Task<IActionResult> GetElectionItemsComments([FromRoute] GetElectionItemCommentsQuery query,int id)
        {
            var response = await _mediator.Send(query);
            if (response.Success)
                return StatusCode(200, response);
            return StatusCode(response.StatusCode, response);
        }


        [SwaggerOperation(Summary = "Add a comment to a specific election item",
        Description = "This endpoint allows authorized users to add a comment to an election item. " +
                  "The election item's unique ID is provided as a path parameter, and the comment content is included in the request body.")]
        [ProducesResponseType(typeof(GenericResponse<UserCommentDto>),201)] 
        [ProducesResponseType(typeof(GenericResponse<UserCommentDto>),400)] 
        [ProducesResponseType(typeof(GenericResponse<UserCommentDto>),401)] 
        [ProducesResponseType(typeof(GenericResponse<UserCommentDto>),404)] 
        [HttpPost]
        //[Authorize]
        [Route("election-item/{id}/comment")]
        public async Task<IActionResult> AddCommentToElectionItem(AddCommentToElectionItemCommand command, int id)
        {
            if (id <= 0) return StatusCode(400, new GenericResponse<UserCommentDto>
            {
                Success = false,
                StatusCode = 400,
                Message = "One or more validation errors has occurred",
                Errors = new List<string>() { "Wrong Id of comment supplied" }
            });
            command.Id = id;
            var response = await _mediator.Send(command);
            if (response.Success)
                return StatusCode(201,response);
            return StatusCode(response.StatusCode, response);

        }

        [SwaggerOperation(Summary = "Retrieve election items by district",
         Description = "This endpoint retrieves a list of election items located in a specific district. " +
                  "The district name is provided as a path parameter and is used to filter the results. " +
                  "If no items are found in the specified district, an empty list is returned.")]
        [ProducesResponseType(typeof(GenericResponseWithList<ElectionItemDto>), 200)]
        [ProducesResponseType(typeof(GenericResponseWithList<ElectionItemDto>), 400)]
    
        [HttpGet]
        [Route("election-items/district/{district}")]
        public async Task<IActionResult> GetElectionItemsByDistrict([FromRoute] GetElectionItemsByDistrictQuery query, string district)
        {
            var response = await _mediator.Send(query);

            if (response.Success)
                //return Ok(new { district, response });
                return Ok(response);
            return StatusCode(response.StatusCode, response);
        }

        [SwaggerOperation(Summary = "Retrieve expired election items",
        Description = "This endpoint retrieves a list of election items that have expired. " +
                  "An expired election item is one whose end date or validity period has passed.")]
        [ProducesResponseType(typeof(GenericResponseWithList<ElectionItemDto>),200)] 
        [ProducesResponseType(typeof(GenericResponseWithList<ElectionItemDto>),400)] 
      //  [ProducesResponseType(typeof(GenericResponseWithList<ElectionItemDto>),404)] 
        [HttpGet]
        [Route("election-items/expired")]
        public async Task<IActionResult> GetExpiredElectionItems([FromRoute]GetExpiredElectionItemsQuery query)
        {
            query.UserOnly = false;
            var response = await _mediator.Send(query);
            if (response.Success)
                return Ok(response);
            return StatusCode(response.StatusCode,response);
        }


        [SwaggerOperation(Summary = "Retrieve election items expiring soon",
        Description = "This endpoint retrieves a list of election items that are set to expire within a specified number of days. " +
                  "The number of days is provided as a path parameter.")]
        [ProducesResponseType(typeof(GenericResponseWithList<ElectionItemDto>), 200)]
        [ProducesResponseType(typeof(GenericResponseWithList<ElectionItemDto>), 400)]
       // [ProducesResponseType(typeof(GenericResponseWithList<ElectionItemDto>), 404)]
        [HttpGet]
        [Route("election-items/expiring-soon/{days}")]
        public async Task<IActionResult> GetSoonExpiringElectionItems([FromRoute] GetSoonExpiringElectionItemsQuery query, int days)
        {

            query.UserOnly = false; 
            var response = await _mediator.Send(query);

            if (response.Success)
                return Ok(response);
            return StatusCode(response.StatusCode, response);
        }
        

    }
}
