using Azure.Core;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using ElectionMaterialManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ElectionMaterialManager.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class AuthenticationController: ControllerBase
    {
        private readonly ElectionMaterialManagerContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AuthService _authService;


        public AuthenticationController(ElectionMaterialManagerContext db,
            UserManager<IdentityUser> userManager, AuthService authService)
        {
            _db = db;
            _userManager = userManager;
            _authService = authService;

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequestDTO request)
        {
            var user = await _userManager.FindByNameAsync(request.Login);
            if (user == null) return Unauthorized();

            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid) return Unauthorized();

            string token = _authService.CreateToken(user);

            return Ok(new {BearerToken = token});        
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterRequestDTO request)
        {
            var userFromDb = await _userManager.FindByNameAsync(request.Username);
            if (userFromDb != null) return Conflict(new { message = "User already exists" });

            var user = new IdentityUser { UserName = request.Username, Email = request.Email };
            var response = await _userManager.CreateAsync(user, request.Password);
            if (!response.Succeeded) return BadRequest(response.Errors);

            return Created();
        }

    }
}
