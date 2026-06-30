using FitnessTracker.Shared.Exceptions;
using Imagekit.Exceptions;
using Imagekit.Models.Files;
using Moq;

namespace FitnessTracker.Application.Tests
{
    public class ImageKitRemoteStorageTests
    {
        [Fact]
        public async Task Upload_ShouldReturnUrl_WhenUploadSucceeded()
        {
            Environment.SetEnvironmentVariable(
                "IMAGEKIT_PRIVATE_KEY",
                "test-key");
            var stream = new MemoryStream();
            var clientMock = new Mock<IImageKitClientWrapper>();
            clientMock
                .Setup(x => x.UploadOnServer(It.IsAny<FileUploadParams>()))
                .ReturnsAsync("https://imagekit.io/photo.jpg");
            var storage = new ImageKitRemoteStorage(clientMock.Object);

            var result = await storage.Upload(stream);

            Assert.Equal("https://imagekit.io/photo.jpg", result);

            clientMock.Verify(
                x => x.UploadOnServer(
                    It.Is<FileUploadParams>(p =>
                        p.File == stream &&
                        p.Folder == "fitness" &&
                        p.FileName.EndsWith(".jpg"))),
                Times.Once);
        }


        [Fact]
        public async Task Upload_ShouldThrow_WhenPrivateKeyIsMissing()
        {
            Environment.SetEnvironmentVariable(
                "IMAGEKIT_PRIVATE_KEY",
                null);
            var clientMock = new Mock<IImageKitClientWrapper>();
            var storage = new ImageKitRemoteStorage(clientMock.Object);

            await Assert.ThrowsAsync<EnviormnetVariableNotFoundException>(
                () => storage.Upload(new MemoryStream()));
            clientMock.Verify(
                x => x.UploadOnServer(It.IsAny<FileUploadParams>()),
                Times.Never);
        }


        [Fact]
        public async Task Upload_ShouldThrow_WhenReturnedUrlIsNull()
        {
            Environment.SetEnvironmentVariable(
                "IMAGEKIT_PRIVATE_KEY",
                "test-key");
            var clientMock = new Mock<IImageKitClientWrapper>();
            clientMock
                .Setup(x => x.UploadOnServer(It.IsAny<FileUploadParams>()))
                .ReturnsAsync((string?)null);
            var storage = new ImageKitRemoteStorage(clientMock.Object);

            await Assert.ThrowsAsync<PhotoStorageException>(
                () => storage.Upload(new MemoryStream()));
        }


        [Fact]
        public async Task Upload_ShouldThrow_WhenImageKit4xxExceptionOccurs()
        {
            Environment.SetEnvironmentVariable(
                "IMAGEKIT_PRIVATE_KEY",
                "test-key");

            var clientMock = new Mock<IImageKitClientWrapper>();

            clientMock
                .Setup(x => x.UploadOnServer(It.IsAny<FileUploadParams>()))
                .ThrowsAsync(new ImageKit4xxException(new HttpRequestException("Unauthorized")) 
                { 
                    StatusCode = System.Net.HttpStatusCode.Unauthorized, 
                    ResponseBody = "Unauthrozed"
                });

            var storage = new ImageKitRemoteStorage(clientMock.Object);

            await Assert.ThrowsAsync<ExternalServerAccessException>(
                () => storage.Upload(new MemoryStream()));
        }


        [Fact]
        public async Task Upload_ShouldThrow_WhenImageKitExceptionOccurs()
        {
            Environment.SetEnvironmentVariable(
                "IMAGEKIT_PRIVATE_KEY",
                "test-key");

            var clientMock = new Mock<IImageKitClientWrapper>();

            clientMock
                .Setup(x => x.UploadOnServer(It.IsAny<FileUploadParams>()))
                .ThrowsAsync(new ImageKitException("Server error"));

            var storage = new ImageKitRemoteStorage(clientMock.Object);

            await Assert.ThrowsAsync<PhotoStorageException>(
                () => storage.Upload(new MemoryStream()));
        }
    }
}
