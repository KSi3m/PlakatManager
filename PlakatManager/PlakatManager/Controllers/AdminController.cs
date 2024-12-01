using ElectionMaterialManager.CQRS.Commands.AdminCommands.AssignAdminRole;
using ElectionMaterialManager.CQRS.Commands.AuthenticationCommands.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

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
