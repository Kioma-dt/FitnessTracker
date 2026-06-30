using FitnessTracker.Application.Interfaces.Images;
using Imagekit.Exceptions;
using Imagekit.Models.Files;

namespace FitnessTracker.Inrastructure.Images.PhotosRemoteStorage
{
    public class ImageKitRemoteStorage
        : IPhotosRemoteStorage
    {
        private readonly IImageKitClientWrapper _client;
        public ImageKitRemoteStorage(IImageKitClientWrapper imageKitClient)
        {
            _client = imageKitClient;
        }
        public async Task<string> Upload(Stream stream)
        {
            var privateKey = Environment.GetEnvironmentVariable("IMAGEKIT_PRIVATE_KEY") 
                ?? throw new EnviormnetVariableNotFoundException("IMAGEKIT_PRIVATE_KEY env variable is not set");
            FileUploadParams parameters = new()
            {
                File = stream,
                FileName = $"{Guid.NewGuid()}.jpg",
                Folder = "fitness"
            };

            try
            {
                var url = await _client.UploadOnServer(parameters);

                if (url is null)
                {
                    throw new PhotoStorageException("Remote storage did not return a url for the uploaded photo");
                }

                return url;
            }
            catch(ImageKit4xxException ex)
            {
                throw new ExternalServerAccessException($"Remote storage returned an error: {ex.Message}");
            }
            catch (ImageKitException ex)
            {
                throw new PhotoStorageException($"Remote storage returned an error: {ex.Message}");
            }
        }
    }
}
