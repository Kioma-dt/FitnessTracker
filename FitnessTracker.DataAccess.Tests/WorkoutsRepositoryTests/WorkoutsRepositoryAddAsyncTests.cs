using FitnessTracker.DataAccess.Repositories;
using FitnessTracker.Entities;
using FitnessTracker.Shared.Enums;
using FitnessTracker.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.DataAccess.Tests.WorkoutsRepositoryTests
{
    public class WorkoutsRepositoryAddAsyncTests
        : WorkoutsRepositoryTestsBase
    {
        [Fact]
        public async Task AddAsync_ShouldAddWorkout_WhenWorkoutNoExists()
        {
            using var context = CreateDbContext();

            var repository = new WorkoutsRepository(context, _orderingApplierMock.Object, _filterBuilderMock.Object);

            var date = DateTime.UtcNow;
            var workout = new Workout
            {
                Id = "1",
                UserId = "user1",
                Title = "Leg day",
                Type = WorkoutType.Strength,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = date,
                Exercises =
                [
                    new Exercise
                    {
                        Name = "Squat",
                        Sets =
                        [
                            new Set(10, 100),
                            new Set(8, 120)
                        ]
                    }
                ],
                ProgressPhotos = ["photo1.jpg"]
            };

            await repository.AddAsync(workout);

            var dbWorkout = await context.Workouts
                .FirstOrDefaultAsync(x => x.Id == "1");

            Assert.NotNull(dbWorkout);
            Assert.Equal("user1", dbWorkout.UserId);
            Assert.Equal("Leg day", dbWorkout.Title);
            Assert.Equal(WorkoutType.Strength, dbWorkout.Type);
            Assert.Equal(TimeSpan.FromMinutes(60), dbWorkout.Duration);
            Assert.Equal(500, dbWorkout.CaloriesBurned);
            Assert.Single(dbWorkout.Exercises);
            Assert.Equal("Squat", dbWorkout.Exercises[0].Name);
            Assert.Single(dbWorkout.ProgressPhotos);
            Assert.Equal("photo1.jpg", dbWorkout.ProgressPhotos[0]);
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenWorkoutExists()
        {
            using var context = CreateDbContext();

            var repository = new WorkoutsRepository(context, _orderingApplierMock.Object, _filterBuilderMock.Object);

            var workout1 = new Workout
            {
                Id = "1",
                UserId = "user1",
                Title = "Leg day",
                Type = WorkoutType.Strength,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = DateTime.UtcNow,
                Exercises =
                [
                    new Exercise
                    {
                        Name = "Squat",
                        Sets =
                        [
                            new Set(10, 100),
                            new Set(8, 120)
                        ]
                    }
                ],
                ProgressPhotos = ["photo1.jpg"]
            };

            context.Workouts.Add(workout1);
            await context.SaveChangesAsync();

            var workout2 = new Workout
            {
                Id = "1",
                UserId = "user1",
                Title = "Leg day",
                Type = WorkoutType.Strength,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = DateTime.UtcNow,
                Exercises =
                [
                    new Exercise
                    {
                        Name = "Squat",
                        Sets =
                        [
                            new Set(10, 100),
                            new Set(8, 120)
                        ]
                    }
                ],
                ProgressPhotos = ["photo1.jpg"]
            };

            await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => repository.AddAsync(workout2));
        }

        [Fact]
        public async Task AddAsync_ShouldAddWorkout_WhenWorkoutIdIsNull()
        {
            using var context = CreateDbContext();

            var repository = new WorkoutsRepository(context, _orderingApplierMock.Object, _filterBuilderMock.Object);

            var date = DateTime.UtcNow;
            var workout = new Workout
            {
                Id = null,
                UserId = "user1",
                Title = "Leg day",
                Type = WorkoutType.Strength,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = date,
                Exercises =
                [
                    new Exercise
                    {
                        Name = "Squat",
                        Sets =
                        [
                            new Set(10, 100),
                            new Set(8, 120)
                        ]
                    }
                ],
                ProgressPhotos = ["photo1.jpg"]
            };

            await repository.AddAsync(workout);

            var dbWorkout = await context.Workouts
                .FirstOrDefaultAsync(x => x.Title == "Leg day");

            Assert.NotNull(dbWorkout);
            Assert.Equal("user1", dbWorkout.UserId);
        }
    }
}
