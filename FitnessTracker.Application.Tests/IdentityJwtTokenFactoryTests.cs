using FitnessTracker.Entities;
using FitnessTracker.Shared.Exceptions.InternalServerError;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FitnessTracker.Application.Tests
{
    public class IdentityJwtTokenFactoryTests
    {
        [Fact]
        public void Create_ShouldReturnJwtToken_WhenUserAndConfigurationAreValid()
        {
            var factory = new IdentityJwtTokenFactory(CreateConfiguration());

            var user = CreateUser();

            var token = factory.Create(user);

            Assert.False(string.IsNullOrWhiteSpace(token));

            var jwt = new JwtSecurityTokenHandler()
                .ReadJwtToken(token);

            Assert.Equal(
                user.Id,
                jwt.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            Assert.Equal(
                user.Name,
                jwt.Claims.First(x => x.Type == ClaimTypes.Name).Value);
        }


        [Theory]
        [InlineData(null, "Roman")]
        [InlineData("123", null)]
        public void Create_ShouldThrow_WhenUserHasInvalidData(
            string? id,
            string? name)
        {
            var factory = new IdentityJwtTokenFactory(CreateConfiguration());

            var user = new User
            {
                Id = id,
                Name = name
            };

            Assert.Throws<ArgumentException>(() => factory.Create(user));
        }


        [Fact]
        public void Create_ShouldThrow_WhenExpiresAfterMinutesIsMissing()
        {
            var factory = new IdentityJwtTokenFactory(
                CreateConfiguration(expiresAfterMinutes: null));

            Assert.Throws<ConfigurationSectionNotFoundException>(
                () => factory.Create(CreateUser()));
        }


        [Fact]
        public void Create_ShouldThrow_WhenExpiresAfterMinutesIsInvalid()
        {
            var factory = new IdentityJwtTokenFactory(
                CreateConfiguration(expiresAfterMinutes: "abc"));

            Assert.Throws<ConfigurationSectionNotFoundException>(
                () => factory.Create(CreateUser()));
        }


        [Fact]
        public void Create_ShouldThrow_WhenJwtKeyIsMissing()
        {
            var factory = new IdentityJwtTokenFactory(
                CreateConfiguration(jwtKey: null));

            Assert.Throws<EnviormnetVariableNotFoundException>(
                () => factory.Create(CreateUser()));
        }


        private User CreateUser()
        {
            return new User
            {
                Id = "123",
                Name = "Roman"
            };
        }


        private IConfiguration CreateConfiguration(
            string? expiresAfterMinutes = "60",
            string? jwtKey = "super-secret-key-super-secret-key",
            string? issuer = "TestIssuer",
            string? audience = "TestAudience")
        {
            var data = new Dictionary<string, string?>
            {
                ["Authentication:ExpiresAfterMinutes"] = expiresAfterMinutes,
                ["Authentication:Issuer"] = issuer,
                ["Authentication:Audience"] = audience,
                ["JWT_KEY"] = jwtKey
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(data)
                .Build();
        }
    }
}
