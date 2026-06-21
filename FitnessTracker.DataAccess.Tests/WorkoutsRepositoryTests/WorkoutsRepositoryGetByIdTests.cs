using FitnessTracker.DataAccess.Repositories;
using FitnessTracker.Entities;
using FitnessTracker.Shared.Enums;

namespace FitnessTracker.DataAccess.Tests.WorkoutsRepositoryTests
{
    public class WorkoutsRepositoryGetByIdTests
        : WorkoutsRepositoryTestsBase
    {

        [Fact]
        public async Task GetByIdAsync_ShouldRetrunWorkout_WhenWorkoutExists()
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

            context.Workouts.Add(workout);
            await context.SaveChangesAsync();

            var dbWorkout = await repository.GetByIdAsync("1");

            Assert.NotNull(dbWorkout);
            Assert.Equal("Leg day", dbWorkout.Title);
            Assert.Single(dbWorkout.Exercises);
            Assert.Equal("Squat", dbWorkout.Exercises[0].Name);
            Assert.Single(dbWorkout.ProgressPhotos);
            Assert.Equal("photo1.jpg", dbWorkout.ProgressPhotos[0]);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenWorkoutNotExists()
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

            context.Workouts.Add(workout);
            await context.SaveChangesAsync();

            var dbWorkout = await repository.GetByIdAsync("2");

            Assert.Null(dbWorkout);
        }
    }
}
