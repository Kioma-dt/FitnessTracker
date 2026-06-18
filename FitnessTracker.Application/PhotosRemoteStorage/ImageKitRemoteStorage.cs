using Imagekit;
using Imagekit.Exceptions;
using Imagekit.Models.Files;

namespace FitnessTracker.Application.PhotosRemoteStorage
{
    public class ImageKitRemoteStorage
        : IPhotosRemoteStorage
    {
        public async Task<string> Upload(FileStream stream)
        {
            var privateKey = Environment.GetEnvironmentVariable("IMAGE_KIT_PRIVATE_KEY") 
                ?? throw new EnviormnetVariableNotFoundException("IMAGE_KIT_PRIVATE_KEY env variable is not set");
            ImageKitClient client = new()
            {
                PrivateKey = privateKey
            };

            FileUploadParams parameters = new()
            {
                File = stream,
                FileName = $"{Guid.NewGuid()}.jpg",
                Folder = "fitness"
            };

            try
            {
                var response = await client.Files.Upload(parameters);

                var url = response.Url;

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
            catch(ImageKitException ex)
            {
                throw new PhotoStorageException($"Remote storage returned an error: {ex.Message}");
            }
        }
    }
}
