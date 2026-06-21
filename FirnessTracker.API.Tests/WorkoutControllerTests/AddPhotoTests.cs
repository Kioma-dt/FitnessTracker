using FitnessTracker.API.Controllers;
using FitnessTracker.Application.PhotosRemoteStorage;
using FitnessTracker.Application.Repositories;
using FitnessTracker.Application.StreamImageChecker;
using FitnessTracker.Entities;
using FitnessTracker.Shared.Exceptions;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;


namespace FirnessTracker.API.Tests.WorkoutControllerTests
{
    public class AddPhotoTests
        : WorkoutControllerTestsBase
    {
        [Fact]
        public async Task AddPhoto_ShouldThrowEntityNotFoundException_WhenWorkoutNotExists()
        {
            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();

            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                Mock.Of<IMapper>());

            var storageMock = new Mock<IPhotosRemoteStorage>();
            var checkerMock = new Mock<IStreamImageChecker>();
            var fileMock = new Mock<IFormFile>();

            var id = "workout1";

            workoutsRepositoryMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync((Workout?)null);


            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                controller.AddPhoto(
                    storageMock.Object,
                    checkerMock.Object,
                    id,
                    fileMock.Object));


            authorizationServiceMock.Verify(
                x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<object>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task AddPhoto_ShouldThrowAccessDeniedException_WhenUserIsNotOwner()
        {
            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();

            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                Mock.Of<IMapper>());

            var storageMock = new Mock<IPhotosRemoteStorage>();
            var checkerMock = new Mock<IStreamImageChecker>();
            var fileMock = new Mock<IFormFile>();

            var workout = new Workout
            {
                Id = "workout1",
                UserId = "user1"
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
                controller.AddPhoto(
                    storageMock.Object,
                    checkerMock.Object,
                    workout.Id,
                    fileMock.Object));


            checkerMock.Verify(
                x => x.IsSteamImage(It.IsAny<Stream>()),
                Times.Never);

            storageMock.Verify(
                x => x.Upload(It.IsAny<Stream>()),
                Times.Never);

            workoutsRepositoryMock.Verify(
                x => x.AddPhotoAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task AddPhoto_ShouldThrowUnsuportedFileFormatException_WhenFileIsNotImage()
        {
            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();

            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                Mock.Of<IMapper>());

            var storageMock = new Mock<IPhotosRemoteStorage>();
            var checkerMock = new Mock<IStreamImageChecker>();
            var fileMock = new Mock<IFormFile>();

            var workout = new Workout
            {
                Id = "workout1"
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

            fileMock
                .SetupGet(x => x.ContentType)
                .Returns("application/pdf");


            await Assert.ThrowsAsync<UnsuportedFileFormatException>(() =>
                controller.AddPhoto(
                    storageMock.Object,
                    checkerMock.Object,
                    workout.Id,
                    fileMock.Object));


            checkerMock.Verify(
                x => x.IsSteamImage(It.IsAny<Stream>()),
                Times.Never);

            storageMock.Verify(
                x => x.Upload(It.IsAny<Stream>()),
                Times.Never);
        }

        [Fact]
        public async Task AddPhoto_ShouldThrowUnprocessableImageException_WhenImageCannotBeDecoded()
        {
            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();

            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                Mock.Of<IMapper>());

            var storageMock = new Mock<IPhotosRemoteStorage>();
            var checkerMock = new Mock<IStreamImageChecker>();
            var fileMock = new Mock<IFormFile>();

            var workout = new Workout
            {
                Id = "workout1"
            };

            var stream = new MemoryStream();

            workoutsRepositoryMock
                .Setup(x => x.GetByIdAsync(workout.Id))
                .ReturnsAsync(workout);

            authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    workout,
                    "WorkoutOwner"))
                .ReturnsAsync(AuthorizationResult.Success());

            fileMock
                .SetupGet(x => x.ContentType)
                .Returns("image/jpeg");

            fileMock
                .Setup(x => x.OpenReadStream())
                .Returns(stream);

            checkerMock
                .Setup(x => x.IsSteamImage(stream))
                .ReturnsAsync(false);


            await Assert.ThrowsAsync<UnprocessableImageException>(() =>
                controller.AddPhoto(
                    storageMock.Object,
                    checkerMock.Object,
                    workout.Id,
                    fileMock.Object));


            storageMock.Verify(
                x => x.Upload(It.IsAny<Stream>()),
                Times.Never);

            workoutsRepositoryMock.Verify(
                x => x.AddPhotoAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task AddPhoto_ShouldUploadPhotoAndAddUrlToWorkout_WhenRequestIsValid()
        {
            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();

            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                Mock.Of<IMapper>());

            var storageMock = new Mock<IPhotosRemoteStorage>();
            var checkerMock = new Mock<IStreamImageChecker>();
            var fileMock = new Mock<IFormFile>();

            var workout = new Workout
            {
                Id = "workout1",
                UserId = "user1"
            };

            var stream = new MemoryStream();
            var url = "https://example.com/photo.jpg";

            workoutsRepositoryMock
                .Setup(x => x.GetByIdAsync(workout.Id))
                .ReturnsAsync(workout);

            authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    workout,
                    "WorkoutOwner"))
                .ReturnsAsync(AuthorizationResult.Success());

            fileMock
                .SetupGet(x => x.ContentType)
                .Returns("image/jpeg");

            fileMock
                .Setup(x => x.OpenReadStream())
                .Returns(stream);

            checkerMock
                .Setup(x => x.IsSteamImage(stream))
                .ReturnsAsync(true);

            storageMock
                .Setup(x => x.Upload(stream))
                .ReturnsAsync(url);

            workoutsRepositoryMock
                .Setup(x => x.AddPhotoAsync(workout.Id, url))
                .Returns(Task.CompletedTask);


            var result = await controller.AddPhoto(
                storageMock.Object,
                checkerMock.Object,
                workout.Id,
                fileMock.Object);


            Assert.NotNull(result);

            checkerMock.Verify(
                x => x.IsSteamImage(stream),
                Times.Once);

            storageMock.Verify(
                x => x.Upload(stream),
                Times.Once);

            workoutsRepositoryMock.Verify(
                x => x.AddPhotoAsync(workout.Id, url),
                Times.Once);
        }
    }
}
