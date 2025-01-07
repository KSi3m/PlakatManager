using Azure.Core;
using ElectionMaterialManager.CQRS.Commands.AuthenticationCommands.Login;
using ElectionMaterialManager.CQRS.Commands.AuthenticationCommands.Register;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using ElectionMaterialManager.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ElectionMaterialManager.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class AuthenticationController: ControllerBase
    {
        private readonly IMediator _mediator;
       
        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SwaggerOperation(Summary = "User login",
        Description = "This endpoint allows a user to log in by providing valid credentials (username and password). " +
                  "If the credentials are correct, a JWT token is returned, which can be used for authenticated requests.")]
        [ProducesResponseType(200)] 
        [ProducesResponseType(400)]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Success)
                return Ok(new { BearerToken = response.Token }); 
            return BadRequest(new { response.Message });
     
        }

        [SwaggerOperation(Summary = "User registration",
        Description = "This endpoint allows a new user to register by providing necessary information such as username, email, and password. " +
                  "If the registration is successful, a success message is returned.")]
        [ProducesResponseType(201)] 
        [ProducesResponseType(409)] 
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Success)
                return Created("",response.Message);
            return Conflict(new { response.Message });
        }

    }
}
