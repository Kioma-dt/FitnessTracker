using FitnessTracker.DataAccess.Repositories;
using FitnessTracker.Entities;

namespace FitnessTracker.DataAccess.Tests.UsersRepositoryTests
{
    public class UsersRepositoryGetByNameTests
        : UsersRepositoryTestsBase
    {
        [Fact]
        public async Task GetByNameAsync_ShouldReturnUser_WhenUserExists()
        {
            using var context = CreateDbContext();

            var repository = new UsersRepository(context);

            var user = new User
            {
                Id = "1",
                Name = "Roman",
                PasswordHash = "hashed_password"
            };
            context.Add(user);
            await context.SaveChangesAsync();

            var dbUser = await repository.GetByNameAsync("Roman");

            Assert.NotNull(dbUser);
            Assert.Equal("1", dbUser.Id);
            Assert.Equal("Roman", dbUser.Name);
            Assert.Equal("hashed_password", dbUser.PasswordHash);
        }

        [Fact]
        public async Task GetByNameAsync_ShouldReturnNull_WhenUserDoesNotExists()
        {
            using var context = CreateDbContext();

            var repository = new UsersRepository(context);

            var user = new User
            {
                Id = "1",
                Name = "Roman",
                PasswordHash = "hashed_password"
            };
            context.Add(user);
            await context.SaveChangesAsync();

            var dbUser = await repository.GetByNameAsync("NotRoman");

            Assert.Null(dbUser);
        }

       
    }
}
