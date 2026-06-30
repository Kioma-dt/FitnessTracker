using FitnessTracker.Application.Interfaces.Images;
using SkiaSharp;
namespace FitnessTracker.Inrastructure.Images.StreamImageChecker
{
    public class SkiaSharpStreamImageChecker
        : IStreamImageChecker
    {
        public async Task<bool> IsSteamImage(Stream stream)
        {
            using var decode = SKBitmap.Decode(stream);

            if (decode is null)
            {
                return false;
            }

            return true;
        }
    }
}
