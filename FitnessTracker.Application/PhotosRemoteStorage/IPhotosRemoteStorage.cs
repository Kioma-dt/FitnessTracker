namespace FitnessTracker.Application.PhotosRemoteStorage
{
    public interface IPhotosRemoteStorage
    {
        Task<string> Upload(FileStream stream);
    }
}
