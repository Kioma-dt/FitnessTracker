using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.DataAccess.Tests.UsersRepositoryTests
{
    public class UsersRepositoryTestsBase
    {
        protected FitnessTrackerDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<FitnessTrackerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new FitnessTrackerDbContext(options);
        }
    }
}
