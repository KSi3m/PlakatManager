using ElectionMaterialManager.CQRS.Commands.AdminCommands.AssignAdminRole;
using ElectionMaterialManager.CQRS.Commands.AuthenticationCommands.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElectionMaterialManager.Controllers
{

    [Route("api/v1/")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SwaggerOperation(Summary = "Assign Admin role to a user",
        Description = "This endpoint allows an Admin to assign the 'Admin' role to a specified user. " +
                  "The user's ID is provided as a path parameter.")]
        [ProducesResponseType(204)] 
        [ProducesResponseType(400)] 
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("user/{userId}/assign-admin-role")]
        public async Task<IActionResult> AssignAdmin(string userId)
        {
            var response = await _mediator.Send(new AssignAdminRoleCommand() { UserId=userId});
            if (response.Success)
                return NoContent();
            return BadRequest(new { response.Message });

        }

    }
}
