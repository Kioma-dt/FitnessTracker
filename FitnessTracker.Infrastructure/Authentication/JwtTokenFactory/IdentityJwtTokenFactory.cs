using FitnessTracker.Application.Interfaces.Authentication;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FitnessTracker.Inrastructure.Authentication.JwtTokenFactory
{
    public class IdentityJwtTokenFactory
        : IJwtTokenFactory
    {
        IConfiguration _config;

        public IdentityJwtTokenFactory(IConfiguration config)
        {
            _config = config;
        }

        public string Create(User user)
        {
            if(user.Id is null || user.Name is null)
            {
                throw new ArgumentException("User id or user name should not be null when creating jwt token");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Name)
            };

            var authOptions = _config.GetSection("Authentication");

            var expiresAfterMinutesSection = authOptions.GetSection("ExpiresAfterMinutes").Value;

            if (expiresAfterMinutesSection is null)
            {
                throw new ConfigurationSectionNotFoundException("ExpiresAfterMinutes should be set in configuration");
            }

            var jwtKeySection = _config.GetSection("JWT_KEY").Value;

            if (jwtKeySection is null)
            {
                throw new EnviormnetVariableNotFoundException("JWT_KEY should be in enviorment variables");
            }

            var key = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                jwtKeySection
                                ));

            if (!UInt32.TryParse(expiresAfterMinutesSection, out var expiresAfterMinutes))
            {
                throw new ConfigurationSectionNotFoundException("ExpiresAfterMinutes should be a valid integer");
            }

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
