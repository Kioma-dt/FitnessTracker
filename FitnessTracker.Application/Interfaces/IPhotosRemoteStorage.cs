namespace FitnessTracker.Application.Interfaces
{
    public interface IPhotosRemoteStorage
    {
        Task<string> Upload(Stream stream);
    }
}
