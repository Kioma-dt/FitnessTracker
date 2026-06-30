namespace FitnessTracker.Application.Interfaces.Images
{
    public interface IStreamImageChecker
    {
        Task<bool> IsSteamImage(Stream stream);
    }
}
