using Imagekit;
using Imagekit.Models.Files;

namespace FitnessTracker.Inrastructure.Images.PhotosRemoteStorage
{
    public interface IImageKitClientWrapper
    {
        Task<string?> UploadOnServer(FileUploadParams parameters);
    }

    public class ImageKitClientWrapper 
        : IImageKitClientWrapper
    {
        private readonly ImageKitClient _client;
        public ImageKitClientWrapper(ImageKitClient client)
        {
            _client = client;
        }
        public async Task<string?> UploadOnServer(FileUploadParams parameters)
        {
            return (await _client.Files.Upload(parameters)).Url;
        }
    }
}
