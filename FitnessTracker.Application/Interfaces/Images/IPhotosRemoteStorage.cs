namespace FitnessTracker.Application.Interfaces.Images
{
    public interface IPhotosRemoteStorage
    {
        Task<string> Upload(Stream stream);
    }
}
