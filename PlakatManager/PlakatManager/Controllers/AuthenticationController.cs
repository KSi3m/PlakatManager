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

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Success)
                return Ok(new { BearerToken = response.Token }); 
            return BadRequest(new { response.Message });
     
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Success)
                return Ok(new { response.Message });
            return Conflict(new { response.Message });
        }

    }
}
