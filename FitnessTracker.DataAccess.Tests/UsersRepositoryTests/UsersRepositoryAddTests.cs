using FitnessTracker.DataAccess.Repositories;
using FitnessTracker.Entities;
using FitnessTracker.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.DataAccess.Tests.UsersRepositoryTests
{
    public class UsersRepositoryAddTests
        : UsersRepositoryTestsBase
    {
        [Fact]
        public async Task AddAsync_ShouldAddUser_WhenUserNoExists()
        {
            using var context = CreateDbContext();

            var repository = new UsersRepository(context);

            var user = new User
            {
                Id = "1",
                Name = "Roman",
                PasswordHash = "hashed_password"
            };

            await repository.AddAsync(user);

            var dbUser = await context.Users
                .FirstOrDefaultAsync(x => x.Id == "1");

            Assert.NotNull(dbUser);
            Assert.Equal("Roman", dbUser.Name);
            Assert.Equal("hashed_password", dbUser.PasswordHash);
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenUserExists()
        {
            using var context = CreateDbContext();

            var repository = new UsersRepository(context);

            var user1 = new User
            {
                Id = "1",
                Name = "Roman",
                PasswordHash = "hashed_password"
            };

            context.Add(user1);
            await context.SaveChangesAsync();

            var user2 = new User
            {
                Id = "1",
                Name = "NotRoman",
                PasswordHash = "not_hashed_password"
            };

            await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => repository.AddAsync(user2));
        }

        [Fact]
        public async Task AddAsync_ShouldAddUser_WhenUserDoesNotHaveId()
        {
            using var context = CreateDbContext();

            var repository = new UsersRepository(context);

            var user = new User
            {
                Id = null,
                Name = "Roman",
                PasswordHash = "hashed_password"
            };

            await repository.AddAsync(user);

            Assert.NotNull(await context.Users.FirstOrDefaultAsync(x => x.Name == "Roman"));
        }

    }
}
