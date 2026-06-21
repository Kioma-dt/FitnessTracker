using FitnessTracker.API.Controllers;
using FitnessTracker.Application.Repositories;
using FitnessTracker.Entities;
using FitnessTracker.Shared.Exceptions;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Moq;
using System.Security.Claims;


namespace FirnessTracker.API.Tests.WorkoutControllerTests
{
    public class DeleteTests
    : WorkoutControllerTestsBase
    {
        [Fact]
        public async Task Delete_ShouldThrow_WhenWorkoutNotExists()
        {
            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                Mock.Of<IMapper>());

            var workoutId = "workout1";

            workoutsRepositoryMock
                .Setup(x => x.GetByIdAsync(workoutId))
                .ReturnsAsync((Workout?)null);


            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                controller.Delete(workoutId));


            authorizationServiceMock.Verify(
                x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<object>(),
                    It.IsAny<string>()),
                Times.Never);

            workoutsRepositoryMock.Verify(
                x => x.DeleteAsync(It.IsAny<string>()),
                Times.Never);
        }


        [Fact]
        public async Task Delete_ShouldThrow_WhenUserIsNotOwner()
        {
            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();

            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                Mock.Of<IMapper>());

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
                controller.Delete(workout.Id));


            workoutsRepositoryMock.Verify(
                x => x.DeleteAsync(It.IsAny<string>()),
                Times.Never);
        }


        [Fact]
        public async Task Delete_ShouldDeleteWorkoutAndReturnNoContent_WhenUserIsOwner()
        {
            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();

            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                Mock.Of<IMapper>());

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
                .ReturnsAsync(AuthorizationResult.Success());

            workoutsRepositoryMock
                .Setup(x => x.DeleteAsync(workout.Id))
                .Returns(Task.CompletedTask);


            var result = await controller.Delete(workout.Id);


            Assert.NotNull(result);

            workoutsRepositoryMock.Verify(
                x => x.DeleteAsync(workout.Id),
                Times.Once);
        }
    }
}
