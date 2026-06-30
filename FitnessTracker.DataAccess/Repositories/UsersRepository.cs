using FitnessTracker.Application.Interfaces.Repositories;
using FitnessTracker.Shared.Exceptions.Conflict;

namespace FitnessTracker.DataAccess.Repositories
{
    public class UsersRepository
        : IUsersRepository
    {
        FitnessTrackerDbContext _dbContext;

        public UsersRepository(FitnessTrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(User user)
        {
            if (user.Id is not null)
            {
                var dbUser = await _dbContext.Users
                    .FirstOrDefaultAsync(x => x.Id == user.Id);

                if (dbUser is not null)
                {
                    throw new EntityAlreadyExistsException($"User with id: {user.Id} alredy exists!");
                }
            }
            else
            {
                user.Id = Guid.NewGuid().ToString();
            }

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User?> GetByNameAsync(string userName)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(x => x.Name == userName);
        }
    }
}
