﻿using Azure.Core;
using ElectionMaterialManager.CQRS.Commands.AuthenticationCommands.Login;
using ElectionMaterialManager.CQRS.Commands.AuthenticationCommands.Register;
using ElectionMaterialManager.CQRS.Responses;
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
        [ProducesResponseType(typeof(TokenResponse),200)] 
        [ProducesResponseType(typeof(TokenResponse),400)]
        [ProducesResponseType(typeof(TokenResponse),401)]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Success)
                return StatusCode(200,response); 
            return StatusCode(response.StatusCode, response);
     
        }

        [SwaggerOperation(Summary = "User registration",
        Description = "This endpoint allows a new user to register by providing necessary information such as username, email, and password. " +
                  "If the registration is successful, a success message is returned.")]
        [ProducesResponseType(typeof(Response),201)]
        [ProducesResponseType(typeof(Response),400)]
        [ProducesResponseType(typeof(Response),409)] 
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Success)
                return StatusCode(201, response);
            return StatusCode(response.StatusCode, response);
        }

    }
}
