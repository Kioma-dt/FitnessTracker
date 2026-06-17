namespace FitnessTracker.Application.Repositories
{
    public interface IUsersRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByNameAsync(string userName);
    }
}
