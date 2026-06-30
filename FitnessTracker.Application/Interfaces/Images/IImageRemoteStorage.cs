namespace FitnessTracker.Application.Interfaces.Images
{
    public interface IImageRemoteStorage
    {
        Task<string> Upload(Stream stream);
    }
}
