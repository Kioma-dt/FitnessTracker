using FitnessTracker.API.Controllers;
using FitnessTracker.Application.JwtTokenFactory;
using FitnessTracker.Application.PasswordHasher;
using FitnessTracker.Application.Repositories;
using FitnessTracker.Entities;
using FitnessTracker.Shared.DTO.Requests;
using FitnessTracker.Shared.DTO.Responses;
using FitnessTracker.Shared.Exceptions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FirnessTracker.API.Tests.UserControllerTests
{
    public class LogInTests
    {
        [Fact]
        public async Task Login_ShouldThrow_WhenUserDoesNotExist()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var jwtTokenFactoryMock = new Mock<IJwtTokenFactory>();
            var mapperMock = new Mock<IMapper>();

            var controller = new UserController(
                usersRepositoryMock.Object,
                passwordHasherMock.Object,
                jwtTokenFactoryMock.Object,
                mapperMock.Object);

            var request = new LoginRequestDTO("Roman", "password");

            usersRepositoryMock
                .Setup(x => x.GetByNameAsync(request.UserName))
                .ReturnsAsync((User?)null);

            await Assert.ThrowsAsync<LoginException>(
                () => controller.Login(request));

            passwordHasherMock.Verify(
                x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);

            jwtTokenFactoryMock.Verify(
                x => x.Create(It.IsAny<User>()),
                Times.Never);
        }

        [Fact]
        public async Task Login_ShouldThrow_WhenUserHasNoPassword()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var jwtTokenFactoryMock = new Mock<IJwtTokenFactory>();
            var mapperMock = new Mock<IMapper>();

            var controller = new UserController(
                usersRepositoryMock.Object,
                passwordHasherMock.Object,
                jwtTokenFactoryMock.Object,
                mapperMock.Object);

            var request = new LoginRequestDTO("Roman", "password");

            var user = new User
            {
                Name = "Roman",
                PasswordHash = null
            };

            usersRepositoryMock
                .Setup(x => x.GetByNameAsync(request.UserName))
                .ReturnsAsync(user);

            await Assert.ThrowsAsync<LoginException>(
                () => controller.Login(request));

            passwordHasherMock.Verify(
                x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
            jwtTokenFactoryMock.Verify(
                x => x.Create(It.IsAny<User>()),
                Times.Never);
        }

        [Fact]
        public async Task Login_ShouldThrowLoginException_WhenPasswordIsWrong()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var jwtTokenFactoryMock = new Mock<IJwtTokenFactory>();
            var mapperMock = new Mock<IMapper>();

            var controller = new UserController(
                usersRepositoryMock.Object,
                passwordHasherMock.Object,
                jwtTokenFactoryMock.Object,
                mapperMock.Object);

            var request = new LoginRequestDTO("Roman", "password");

            var user = new User
            {
                Name = "Roman",
                PasswordHash = "hash"
            };

            usersRepositoryMock
                .Setup(x => x.GetByNameAsync(request.UserName))
                .ReturnsAsync(user);

            passwordHasherMock
                .Setup(x => x.VerifyPassword(request.Password, user.PasswordHash))
                .Returns(false);

            await Assert.ThrowsAsync<LoginException>(
                () => controller.Login(request));

            jwtTokenFactoryMock.Verify(
                x => x.Create(It.IsAny<User>()),
                Times.Never);
        }

        [Fact]
        public async Task Login_ShouldReturnTokenAndUserResponse_WhenCredentialsAreValid()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var jwtTokenFactoryMock = new Mock<IJwtTokenFactory>();
            var mapperMock = new Mock<IMapper>();

            var controller = new UserController(
                usersRepositoryMock.Object,
                passwordHasherMock.Object,
                jwtTokenFactoryMock.Object,
                mapperMock.Object);

            var request = new LoginRequestDTO("Roman", "password");

            var user = new User
            {
                Id = "1",
                Name = "Roman",
                PasswordHash = "hash"
            };

            var userResponse = new UserResponseDTO(
                "1",
                "Roman"
            );

            var token = "jwt-token";

            usersRepositoryMock
                .Setup(x => x.GetByNameAsync(request.UserName))
                .ReturnsAsync(user);

            passwordHasherMock
                .Setup(x => x.VerifyPassword(request.Password, user.PasswordHash))
                .Returns(true);

            jwtTokenFactoryMock
                .Setup(x => x.Create(user))
                .Returns(token);

            mapperMock
                .Setup(x => x.Map<UserResponseDTO>(user))
                .Returns(userResponse);

            var result = await controller.Login(request);

            Assert.IsType<OkObjectResult>(result);
            var resultVal = (result as OkObjectResult)?.Value;
            Assert.IsType<UserTokenResponseDTO>(resultVal);
            var tokenRes = (resultVal as UserTokenResponseDTO)?.Token;
            var userRes = (resultVal as UserTokenResponseDTO)?.User;

            Assert.Equal(token, tokenRes);

            Assert.Equal(userResponse, userRes);

            passwordHasherMock.Verify(
                x => x.VerifyPassword(request.Password, user.PasswordHash),
                Times.Once);

            jwtTokenFactoryMock.Verify(
                x => x.Create(user),
                Times.Once);

            mapperMock.Verify(
                x => x.Map<UserResponseDTO>(user),
                Times.Once);
        }
    }
}
