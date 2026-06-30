using FitnessTracker.API.Controllers;
using FitnessTracker.Application.Interfaces.Authentication;
using FitnessTracker.Application.Interfaces.Repositories;
using FitnessTracker.Entities;
using FitnessTracker.Shared.DTO.Requests;
using FitnessTracker.Shared.DTO.Responses;
using FitnessTracker.Shared.Exceptions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FirnessTracker.API.Tests.UserControllerTests
{
    public class RegisterTests
    {
        [Fact]
        public async Task Register_ShouldThrow_WhenUserAlreadyExists()
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

            var request = new RegisterRequestDTO(
                "Roman",
                "password");

            var existingUser = new User(
                "Roman",
                "hash");

            usersRepositoryMock
                .Setup(x => x.GetByNameAsync(request.UserName))
                .ReturnsAsync(existingUser);

            await Assert.ThrowsAsync<EntityAlreadyExistsException>(
                () => controller.Register(request));
            passwordHasherMock.Verify(
                x => x.HashPassword(It.IsAny<string>()),
                Times.Never);
            usersRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<User>()),
                Times.Never);
        }


        [Fact]
        public async Task Register_ShouldCreateUserAndReturnCreated_WhenRequestIsValid()
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

            var request = new RegisterRequestDTO(
                "Roman",
                "password");

            var passwordHash = "hashedPassword";

            var responseDto = new UserResponseDTO(
                "1",
                "Roman");

            usersRepositoryMock
                .Setup(x => x.GetByNameAsync(request.UserName))
                .ReturnsAsync((User?)null);

            passwordHasherMock
                .Setup(x => x.HashPassword(request.Password))
                .Returns(passwordHash);

            mapperMock
                .Setup(x => x.Map<UserResponseDTO>(It.IsAny<User>()))
                .Returns(responseDto);

            var result = await controller.Register(request);

            Assert.IsType<CreatedAtRouteResult>(result);
            var resultValue = (result as CreatedAtRouteResult)?.Value;
            Assert.Equal(responseDto, resultValue);

            passwordHasherMock.Verify(
                x => x.HashPassword(request.Password),
                Times.Once);

            usersRepositoryMock.Verify(
                x => x.AddAsync(It.Is<User>(u =>
                    u.Name == request.UserName &&
                    u.PasswordHash == passwordHash)),
                Times.Once);

            mapperMock.Verify(
                x => x.Map<UserResponseDTO>(It.IsAny<User>()),
                Times.Once);
        }
    }
}
