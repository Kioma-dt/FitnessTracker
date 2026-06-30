using FitnessTracker.API.Cache;
using FitnessTracker.API.Controllers;
using FitnessTracker.Application.Interfaces.Repositories;
using FitnessTracker.Entities;
using FitnessTracker.Shared.DTO.Responses;
using FitnessTracker.Shared.Enums;
using FitnessTracker.Shared.Exceptions;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;


namespace FirnessTracker.API.Tests.WorkoutControllerTests
{
    public class GetByIdTests
    : WorkoutControllerTestsBase
    {
        [Fact]
        public async Task GetById_ShouldThrow_WhenWorkoutNotExists()
        {
            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            var mapperMock = new Mock<IMapper>();
            var eTagGeneratorMock = new Mock<IETagGenerator>();

            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                eTagGeneratorMock.Object,
                mapperMock.Object);


            var workoutId = "workout1";

            workoutsRepositoryMock
                .Setup(x => x.GetByIdAsync(workoutId))
                .ReturnsAsync((Workout?)null);


            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                controller.GetById(workoutId));


            authorizationServiceMock.Verify(x =>
                x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<object>(),
                    It.IsAny<string>()),
                Times.Never);

            mapperMock.Verify(
                x => x.Map<WorkoutResponseDTO>(It.IsAny<Workout>()),
                Times.Never);
        }


        [Fact]
        public async Task GetById_ShouldThrow_WhenUserIsNotOwner()
        {
            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            var mapperMock = new Mock<IMapper>();
            var eTagGeneratorMock = new Mock<IETagGenerator>();

            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                eTagGeneratorMock.Object,
                mapperMock.Object);



            var workout = new Workout
            {
                Id = "workout1",
                UserId = "user1",
                Title = "Leg day"
            };


            workoutsRepositoryMock
                .Setup(x => x.GetByIdAsync(workout.Id))
                .ReturnsAsync(workout);

            authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    workout,
                    "WorkoutOwner"))
                .ReturnsAsync(AuthorizationResult.Failed());


            await Assert.ThrowsAsync<AccessDeniedException>(() =>
                controller.GetById(workout.Id));


            mapperMock.Verify(
                x => x.Map<WorkoutResponseDTO>(It.IsAny<Workout>()),
                Times.Never);
        }


        [Fact]
        public async Task GetById_ShouldReturnWorkoutResponse_WhenUserIsOwner()
        {
            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            var mapperMock = new Mock<IMapper>();
            var eTagGeneratorMock = new Mock<IETagGenerator>();

            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                eTagGeneratorMock.Object,
                mapperMock.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };



            var workout = new Workout
            {
                Id = "workout1",
                UserId = "user1",
                Title = "Leg day",
                Type = WorkoutType.Strength,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = DateTime.UtcNow
            };

            var response = new WorkoutResponseDTO(
                "workout1",
                "Leg day",
                WorkoutType.Strength,
                60,
                500,
                workout.WorkoutDate,
                new List<ExerciseResponseDTO>(),
                new List<string>()
            );


            workoutsRepositoryMock
                .Setup(x => x.GetByIdAsync(workout.Id))
                .ReturnsAsync(workout);

            authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    workout,
                    "WorkoutOwner"))
                .ReturnsAsync(AuthorizationResult.Success());

            mapperMock
                .Setup(x => x.Map<WorkoutResponseDTO>(workout))
                .Returns(response);


            var result = await controller.GetById(workout.Id);

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response, (result as OkObjectResult)!.Value);

            authorizationServiceMock.Verify(
                x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    workout,
                    "WorkoutOwner"),
                Times.Once);

            mapperMock.Verify(
                x => x.Map<WorkoutResponseDTO>(workout),
                Times.Once);
        }
    }
}
