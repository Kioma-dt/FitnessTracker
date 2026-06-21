using FitnessTracker.API.Controllers;
using FitnessTracker.Application.Repositories;
using FitnessTracker.Entities;
using FitnessTracker.Shared.DTO.Requests;
using FitnessTracker.Shared.DTO.Responses;
using FitnessTracker.Shared.Enums;
using FitnessTracker.Shared.Exceptions;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace FirnessTracker.API.Tests.WorkoutControllerTests
{
    public class CreateTests
        : WorkoutControllerTestsBase
    {
        [Fact]
        public async Task CreateWorkout_ShouldThrow_WhenUserIdClaimMissing()
        {
            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            var mapperMock = new Mock<IMapper>();

            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                mapperMock.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var request = new WorkoutCreateRequestDTO(
                "Leg day",
                WorkoutType.Strength,
                60,
                500,
                DateTime.UtcNow,
                new List<ExerciseCreateRequestDTO>
                {
                    new ExerciseCreateRequestDTO(
                        "Bench Press",
                        new List<SetCreateRequestDTO>
                        {
                            new SetCreateRequestDTO(),
                            new SetCreateRequestDTO()
                        })
                },
                progressPhotos: new List<string>
                {
                    "photo1.jpg"
                });

            await Assert.ThrowsAsync<NoInfoInJWTTokenExeption>(() =>
                controller.CreateWorkout(request));

            mapperMock.Verify(
                x => x.Map<Workout>(It.IsAny<WorkoutCreateRequestDTO>()),
                Times.Never);

            workoutsRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Workout>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateWorkout_ShouldCreateWorkoutAndReturnCreated_WhenRequestIsValid()
        {

            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            var mapperMock = new Mock<IMapper>();

            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                mapperMock.Object);

            var userId = "user1";

            SetupUser(userId, controller);

            var request = new WorkoutCreateRequestDTO(
                "Leg day",
                WorkoutType.Strength,
                60,
                500,
                DateTime.UtcNow,
                new List<ExerciseCreateRequestDTO>
                {
                    new ExerciseCreateRequestDTO(
                        "Bench Press",
                        new List<SetCreateRequestDTO>
                        {
                            new SetCreateRequestDTO(),
                            new SetCreateRequestDTO()
                        })
                },
                progressPhotos: new List<string>
                {
                    "photo1.jpg"
                });

            var workout = new Workout
            {
                Id = "workout1",
                UserId = userId,
                Title = request.Title
            };

            var response = new WorkoutResponseDTO(
                "workout1",
                "Chest training",
                 WorkoutType.Strength,
                 60,
                 500,
                 DateTime.UtcNow,
                 new List<ExerciseResponseDTO>
                {
                    new ExerciseResponseDTO(
                        "Bench Press",
                        new List<SetResponseDTO>
                        {
                            new SetResponseDTO(100, 10),
                            new SetResponseDTO(90, 12)
                        })
                },
                 new List<string>
                {
                    "https://example.com/photo1.jpg"
                });

            mapperMock
                .Setup(x => x.Map<Workout>(
                    It.Is<WorkoutCreateRequestDTO>(r =>
                        r.GetUserId() == userId)))
                .Returns(workout);

            workoutsRepositoryMock
                .Setup(x => x.AddAsync(workout))
                .Returns(Task.CompletedTask);

            mapperMock
                .Setup(x => x.Map<WorkoutResponseDTO>(workout))
                .Returns(response);

            var result = await controller.CreateWorkout(request);


            Assert.Equal(response, result.Value);

            mapperMock.Verify(
                x => x.Map<Workout>(
                    It.Is<WorkoutCreateRequestDTO>(r =>
                        r.GetUserId() == userId)),
                Times.Once);

            workoutsRepositoryMock.Verify(
                x => x.AddAsync(workout),
                Times.Once);

            mapperMock.Verify(
                x => x.Map<WorkoutResponseDTO>(workout),
                Times.Once);
        }
    }
}
