using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ElectionMaterialManager.Entities;

namespace ElectionMaterialManager.Services
{
    public class AuthService: IAuthService
    {
        public readonly JwtSettings _jwtSettings;
        private readonly UserManager<User> _userManager;

        public AuthService(IOptions<JwtSettings> settings, UserManager<User> userManager) {
            _jwtSettings = settings.Value;    
            _userManager = userManager;
        }    

        public async Task<string> CreateToken(User user)
        {
            var issuer = _jwtSettings.Issuer;
            var audience = _jwtSettings.Audience;
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.SecretKey));

            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {               new Claim("Id", Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Email, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };


            claims.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x)));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(60),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials(key,
                       SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return jwtToken;

        }


    }
}
