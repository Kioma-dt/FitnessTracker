using FitnessTracker.API.Controllers;
using FitnessTracker.Application.Repositories;
using FitnessTracker.Entities;
using FitnessTracker.Shared.DTO.Requests;
using FitnessTracker.Shared.Exceptions;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Moq;
using System.Security.Claims;


namespace FirnessTracker.API.Tests.WorkoutControllerTests
{
    public class AddExerciseTests
    : WorkoutControllerTestsBase
    {
        [Fact]
        public async Task AddExercise_Should_WhenWorkoutNotExists()
        {
            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            var mapperMock = new Mock<IMapper>();

            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                mapperMock.Object);

            var workoutId = "workout1";

            var request = new ExerciseCreateRequestDTO(
                "Bench Press",
                new List<SetCreateRequestDTO>());

            workoutsRepositoryMock
                .Setup(x => x.GetByIdAsync(workoutId))
                .ReturnsAsync((Workout?)null);


            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                controller.AddExercise(workoutId, request));


            authorizationServiceMock.Verify(
                x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<object>(),
                    It.IsAny<string>()),
                Times.Never);

            mapperMock.Verify(
                x => x.Map<Exercise>(It.IsAny<ExerciseCreateRequestDTO>()),
                Times.Never);

            workoutsRepositoryMock.Verify(
                x => x.AddExerciseAsync(
                    It.IsAny<string>(),
                    It.IsAny<Exercise>()),
                Times.Never);
        }


        [Fact]
        public async Task AddExercise_ShouldThrow_WhenUserIsNotOwner()
        {
            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            var mapperMock = new Mock<IMapper>();

            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                mapperMock.Object);

            var workout = new Workout
            {
                Id = "workout1",
                UserId = "user1",
                Title = "Leg day"
            };

            var request = new ExerciseCreateRequestDTO(
                "Bench Press",
                new List<SetCreateRequestDTO>());

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
                controller.AddExercise(workout.Id, request));


            mapperMock.Verify(
                x => x.Map<Exercise>(It.IsAny<ExerciseCreateRequestDTO>()),
                Times.Never);

            workoutsRepositoryMock.Verify(
                x => x.AddExerciseAsync(
                    It.IsAny<string>(),
                    It.IsAny<Exercise>()),
                Times.Never);
        }


        [Fact]
        public async Task AddExercise_ShouldAddExerciseAndReturnNoContent_WhenUserIsOwner()
        {
            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            var mapperMock = new Mock<IMapper>();

            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                mapperMock.Object);

            var workout = new Workout
            {
                Id = "workout1",
                UserId = "user1",
                Title = "Leg day"
            };

            var request = new ExerciseCreateRequestDTO(
                "Bench Press",
                new List<SetCreateRequestDTO>
                {
                    new SetCreateRequestDTO(10, 15)
                });

            var exercise = new Exercise
            {
                Name = "Bench Press"
            };


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
                .Setup(x => x.Map<Exercise>(request))
                .Returns(exercise);

            workoutsRepositoryMock
                .Setup(x => x.AddExerciseAsync(workout.Id, exercise))
                .Returns(Task.CompletedTask);


            var result = await controller.AddExercise(workout.Id, request);


            Assert.NotNull(result);

            mapperMock.Verify(
                x => x.Map<Exercise>(request),
                Times.Once);

            workoutsRepositoryMock.Verify(
                x => x.AddExerciseAsync(workout.Id, exercise),
                Times.Once);
        }
    }
}
