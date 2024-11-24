using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ElectionMaterialManager.Services
{
    public class AuthService: IAuthService
    {
        public readonly JwtSettings _jwtSettings;

        public AuthService(IOptions<JwtSettings> settings) {
            _jwtSettings = settings.Value;    
        }    

        public string CreateToken(IdentityUser user)
        {
            var issuer = _jwtSettings.Issuer;
            var audience = _jwtSettings.Audience;
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.SecretKey));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                    Subject = new ClaimsIdentity(new[]
                    {
                            new Claim("Id", Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Email, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        }),
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
