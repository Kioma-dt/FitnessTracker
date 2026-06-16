using FitnessTracker.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

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

        public async Task<User?> GetByNameAsync(string userName)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(x => x.Name == userName);
        }
    }
}
