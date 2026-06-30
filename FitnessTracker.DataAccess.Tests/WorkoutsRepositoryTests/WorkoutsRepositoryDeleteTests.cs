using FitnessTracker.DataAccess.Repositories;
using FitnessTracker.Entities;
using FitnessTracker.Shared.Enums;
using FitnessTracker.Shared.Exceptions.NotFound;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.DataAccess.Tests.WorkoutsRepositoryTests
{
    public class WorkoutsRepositoryDeleteTests
: WorkoutsRepositoryTestsBase
    {

        [Fact]
        public async Task DeleteAsync_ShouldDeleteWorkout_WhenWorkoutExists()
        {
            using var context = CreateDbContext();

            var repository = new WorkoutsRepository(context, _orderingApplierMock.Object, _filterBuilderMock.Object);

            var date = DateTime.UtcNow;
            var workout1 = new Workout
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
            var workout2 = new Workout
            {
                Id = "2",
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

            context.Workouts.Add(workout1);
            context.Workouts.Add(workout2);
            await context.SaveChangesAsync();

            await repository.DeleteAsync("1");

            var dbWorkout = await context.Workouts
                .FirstOrDefaultAsync(x => x.Id == "1");

            Assert.Null(dbWorkout);
            Assert.Equal(1, context.Workouts.Count());
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenWorkoutNotExists()
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

            await Assert.ThrowsAsync<EntityNotFoundException>(() => repository.DeleteAsync("2"));
        }

    }
}
