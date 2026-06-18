using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FitnessTracker.Application.JwtTokenFactory
{
    public class IdentityJwtTokenFactory(IConfiguration config)
        : IJwtTokenFactory
    {
        IConfiguration _config = config;
        public string Create(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id ?? String.Empty),
                new Claim(ClaimTypes.Name, user.Name ?? String.Empty)
            };

            var authOptions = _config.GetSection("Authentication");
            var key = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                _config.GetSection("JWT_KEY").Value
                                ?? String.Empty
                                ));
            var expiresAfterMinutes = Int32.Parse(authOptions["EnspiresAfterMinutes"] ?? String.Empty);

            var jwt = new JwtSecurityToken(
                issuer: authOptions["Issuer"],
                audience: authOptions["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(expiresAfterMinutes)),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
