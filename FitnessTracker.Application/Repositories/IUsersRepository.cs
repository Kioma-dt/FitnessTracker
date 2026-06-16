namespace FitnessTracker.Application.Repositories
{
    public interface IUsersRepository
    {
        Task<User?> GetByNameAsync(string userName);
    }
}
