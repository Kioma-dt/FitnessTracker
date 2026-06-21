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
using Moq;
using System.Security.Claims;


namespace FirnessTracker.API.Tests.WorkoutControllerTests
{
    public class PatchTests
    : WorkoutControllerTestsBase
    {
        [Fact]
        public async Task Patch_ShouldThrowEntityNotFoundException_WhenWorkoutNotExists()
        {
            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            var mapperMock = new Mock<IMapper>();

            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                mapperMock.Object);

            var workoutId = "workout1";

            var request = new WorkoutPatchRequestDTO(
                "New title",
                WorkoutType.Strength,
                60,
                500,
                DateTime.UtcNow,
                null,
                null);

            workoutsRepositoryMock
                .Setup(x => x.GetByIdAsync(workoutId))
                .ReturnsAsync((Workout?)null);


            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                controller.Patch(workoutId, request));


            authorizationServiceMock.Verify(
                x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<object>(),
                    It.IsAny<string>()),
                Times.Never);

            mapperMock.Verify(
                x => x.Map<WorkoutUpdateDTO>(It.IsAny<WorkoutPatchRequestDTO>()),
                Times.Never);
        }


        [Fact]
        public async Task Patch_ShouldThrowAccessDeniedException_WhenUserIsNotOwner()
        {
            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            var mapperMock = new Mock<IMapper>();

            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                mapperMock.Object);

            var workoutId = "workout1";

            var request = new WorkoutPatchRequestDTO(
                "New title",
                WorkoutType.Strength,
                60,
                500,
                DateTime.UtcNow,
                null,
                null);

            var workout = new Workout
            {
                Id = workoutId,
                UserId = "user1",
                Title = "Old title"
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
                controller.Patch(workoutId, request));


            mapperMock.Verify(
                x => x.Map<WorkoutUpdateDTO>(It.IsAny<WorkoutPatchRequestDTO>()),
                Times.Never);

            workoutsRepositoryMock.Verify(
                x => x.UpdateAsync(
                    It.IsAny<string>(),
                    It.IsAny<WorkoutUpdateDTO>()),
                Times.Never);
        }


        [Fact]
        public async Task Patch_ShouldUpdateWorkoutAndReturnOk_WhenUserIsOwner()
        {
            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            var mapperMock = new Mock<IMapper>();

            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                mapperMock.Object);

            var workoutId = "workout1";

            var request = new WorkoutPatchRequestDTO(
                "New title",
                WorkoutType.Strength,
                60,
                500,
                DateTime.UtcNow,
                null,
                null);

            var workout = new Workout
            {
                Id = workoutId,
                UserId = "user1",
                Title = "Old title"
            };

            var updateDto = new WorkoutUpdateDTO(
                Title: "New title",
                Type: WorkoutType.Strength,
                Duration: TimeSpan.FromMinutes(60),
                CaloriesBurned: 500,
                WorkoutDate: DateTime.UtcNow,
                Exercises: null,
                ProgressPhotos: null);

            var updatedWorkout = new Workout
            {
                Id = workoutId,
                UserId = "user1",
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
                .Returns(updateDto);

            workoutsRepositoryMock
                .Setup(x => x.UpdateAsync(workoutId, updateDto))
                .ReturnsAsync(updatedWorkout);

            mapperMock
                .Setup(x => x.Map<WorkoutResponseDTO>(updatedWorkout))
                .Returns(response);


            var result = await controller.Patch(workoutId, request);


            Assert.Equal(response, result.Value);

            workoutsRepositoryMock.Verify(
                x => x.UpdateAsync(workoutId, updateDto),
                Times.Once);

            mapperMock.Verify(
                x => x.Map<WorkoutResponseDTO>(updatedWorkout),
                Times.Once);
        }
    }
}
