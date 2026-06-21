using FitnessTracker.API.Controllers;
using FitnessTracker.Application.Repositories;
using FitnessTracker.Entities;
using FitnessTracker.Shared.DTO;
using FitnessTracker.Shared.DTO.Requests;
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
    public class PutTests
    : WorkoutControllerTestsBase
    {
        [Fact]
        public async Task Put_ShouldThrow_WhenUserIdClaimMissing()
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

            var request = new WorkoutUpdateRequestDTO(
                "Leg day",
                WorkoutType.Strength,
                60,
                500,
                DateTime.UtcNow,
                new List<ExerciseCreateRequestDTO>
                {
                    new ExerciseCreateRequestDTO(
                        "Squats",
                        new List<SetCreateRequestDTO>
                        {
                            new SetCreateRequestDTO(100, 10),
                            new SetCreateRequestDTO(120, 8)
                        })
                },
                new List<string>
                {
                    "https://example.com/photo1.jpg",
                    "https://example.com/photo2.jpg"
                });

            await Assert.ThrowsAsync<NoInfoInJWTTokenExeption>(() =>
                controller.Put("workout1", request));

            workoutsRepositoryMock.Verify(
                x => x.GetByIdAsync(It.IsAny<string>()),
                Times.Never);

            mapperMock.Verify(
                x => x.Map<Workout>(It.IsAny<WorkoutUpdateRequestDTO>()),
                Times.Never);
        }


        [Fact]
        public async Task Put_ShouldCreateWorkoutAndReturnCreated_WhenWorkoutNotExists()
        {
            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            var mapperMock = new Mock<IMapper>();

            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                mapperMock.Object);

            var userId = "user1";
            var workoutId = "workout1";

            SetupUser(userId, controller);

            var request = new WorkoutUpdateRequestDTO(
                "Leg day",
                WorkoutType.Strength,
                60,
                500,
                DateTime.UtcNow,
                new List<ExerciseCreateRequestDTO>
                {
                    new ExerciseCreateRequestDTO(
                        "Squats",
                        new List<SetCreateRequestDTO>
                        {
                            new SetCreateRequestDTO(100, 10),
                            new SetCreateRequestDTO(120, 8)
                        })
                },
                new List<string>
                {
                    "https://example.com/photo1.jpg",
                    "https://example.com/photo2.jpg"
                });

            var workout = new Workout
            {
                Title = "New workout"
            };

            var response = new WorkoutResponseDTO(
                workoutId,
                "New workout",
                WorkoutType.Strength,
                60,
                500,
                DateTime.UtcNow,
                new List<ExerciseResponseDTO>(),
                new List<string>());

            workoutsRepositoryMock
                .Setup(x => x.GetByIdAsync(workoutId))
                .ReturnsAsync((Workout?)null);

            mapperMock
                .Setup(x => x.Map<Workout>(request))
                .Returns(workout);

            workoutsRepositoryMock
                .Setup(x => x.AddAsync(
                    It.Is<Workout>(w => w.Id == workoutId)))
                .Returns(Task.CompletedTask);

            mapperMock
                .Setup(x => x.Map<WorkoutResponseDTO>(workout))
                .Returns(response);


            var result = await controller.Put(workoutId, request);


            workoutsRepositoryMock.Verify(
                x => x.AddAsync(
                    It.Is<Workout>(w => w.Id == workoutId)),
                Times.Once);

            authorizationServiceMock.Verify(
                x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<object>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task Put_ShouldThrow_WhenUserIsNotOwner()
        {
            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            var mapperMock = new Mock<IMapper>();

            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                mapperMock.Object);

            var userId = "user1";
            var workoutId = "workout1";

            SetupUser(userId, controller);

            var request = new WorkoutUpdateRequestDTO(
                "Leg day",
                WorkoutType.Strength,
                60,
                500,
                DateTime.UtcNow,
                new List<ExerciseCreateRequestDTO>
                {
                    new ExerciseCreateRequestDTO(
                        "Squats",
                        new List<SetCreateRequestDTO>
                        {
                            new SetCreateRequestDTO(100, 10),
                            new SetCreateRequestDTO(120, 8)
                        })
                },
                new List<string>
                {
                    "https://example.com/photo1.jpg",
                    "https://example.com/photo2.jpg"
                });

            var workout = new Workout
            {
                Id = workoutId,
                UserId = "anotherUser",
                Title = "Leg day"
            };

            workoutsRepositoryMock
                .Setup(x => x.GetByIdAsync(workoutId))
                .ReturnsAsync(workout);

            authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    workout,
                    "WorkoutOwner"))
                .ReturnsAsync(AuthorizationResult.Failed());


            await Assert.ThrowsAsync<AccessDeniedException>(() =>
                controller.Put(workoutId, request));


            mapperMock.Verify(
                x => x.Map<WorkoutUpdateDTO>(It.IsAny<WorkoutUpdateRequestDTO>()),
                Times.Never);

            workoutsRepositoryMock.Verify(
                x => x.UpdateAsync(
                    It.IsAny<string>(),
                    It.IsAny<WorkoutUpdateDTO>()),
                Times.Never);
        }

        [Fact]
        public async Task Put_ShouldUpdateWorkoutAndReturnOk_WhenUserIsOwner()
        {
            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            var mapperMock = new Mock<IMapper>();

            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                mapperMock.Object);

            var userId = "user1";
            var workoutId = "workout1";

            SetupUser(userId, controller);

            var request = new WorkoutUpdateRequestDTO(
                "Leg day",
                WorkoutType.Strength,
                60,
                500,
                DateTime.UtcNow,
                new List<ExerciseCreateRequestDTO>
                {
                    new ExerciseCreateRequestDTO(
                        "Squats",
                        new List<SetCreateRequestDTO>
                        {
                            new SetCreateRequestDTO(100, 10),
                            new SetCreateRequestDTO(120, 8)
                        })
                },
                new List<string>
                {
                    "https://example.com/photo1.jpg",
                    "https://example.com/photo2.jpg"
                });

            var workout = new Workout
            {
                Id = workoutId,
                UserId = userId,
                Title = "Old title"
            };

            var workoutUpdateDTO = new WorkoutUpdateDTO(
                Title: "New title"
            );

            var updatedWorkout = new Workout
            {
                Id = workoutId,
                UserId = userId,
                Title = "New title"
            };

            var response = new WorkoutResponseDTO(
                workoutId,
                "New title",
                WorkoutType.Strength,
                60,
                500,
                DateTime.UtcNow,
                new List<ExerciseResponseDTO>(),
                new List<string>());

            workoutsRepositoryMock
                .Setup(x => x.GetByIdAsync(workoutId))
                .ReturnsAsync(workout);

            authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    workout,
                    "WorkoutOwner"))
                .ReturnsAsync(AuthorizationResult.Success());

            mapperMock
                .Setup(x => x.Map<WorkoutUpdateDTO>(request))
                .Returns(workoutUpdateDTO);

            workoutsRepositoryMock
                .Setup(x => x.UpdateAsync(workoutId, workoutUpdateDTO))
                .ReturnsAsync(updatedWorkout);

            mapperMock
                .Setup(x => x.Map<WorkoutResponseDTO>(updatedWorkout))
                .Returns(response);


            var result = await controller.Put(workoutId, request);


            workoutsRepositoryMock.Verify(
                x => x.UpdateAsync(workoutId, workoutUpdateDTO),
                Times.Once);

            mapperMock.Verify(
                x => x.Map<WorkoutResponseDTO>(updatedWorkout),
                Times.Once);
        }
    }
}
