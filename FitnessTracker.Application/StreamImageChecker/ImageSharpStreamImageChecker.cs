using SixLabors.ImageSharp;

namespace FitnessTracker.Application.StreamImageChecker
{
    public class ImageSharpStreamImageChecker
        : IStreamImageChecker
    {
        public async Task<bool> IsSteamImage(Stream stream)
        {
            try
            {
                using var image = await Image.LoadAsync(stream);
                return true;
            }
            catch(ImageFormatException ex)
            {
                return false;
            }
        }
    }
}
