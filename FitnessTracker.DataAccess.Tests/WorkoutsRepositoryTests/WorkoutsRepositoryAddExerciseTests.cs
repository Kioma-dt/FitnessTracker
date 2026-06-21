using FitnessTracker.DataAccess.Repositories;
using FitnessTracker.Entities;
using FitnessTracker.Shared.Enums;
using FitnessTracker.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.DataAccess.Tests.WorkoutsRepositoryTests
{
    public class WorkoutsRepositoryAddExerciseTests
        : WorkoutsRepositoryTestsBase
    {
        [Fact]
        public async Task AddExerciseAsync_ShouldAddExercise_WhenWorkoutExists()
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

            var exercise = new Exercise
            {
                Name = "Lunges",
                Sets =
                [
                    new Set(12, 50),
                    new Set(10, 60)
                ]
            };

            await repository.AddExerciseAsync("1", exercise);

            var dbWorkout = await context.Workouts
                .FirstOrDefaultAsync(x => x.Id == "1");

            Assert.NotNull(dbWorkout);
            Assert.Equal(2, dbWorkout.Exercises.Count);
            var exercisesdb = dbWorkout.Exercises.OrderBy(x => x.Name).ToList();
            Assert.Equal("Lunges", exercisesdb[0].Name);
            Assert.Equal("Squat", exercisesdb[1].Name);
        }

        [Fact]
        public async Task AddExerciseAsync_ShouldThrow_WhenWorkoutNotExists()
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

            var exercise = new Exercise
            {
                Name = "Lunges",
                Sets =
                [
                    new Set(12, 50),
                    new Set(10, 60)
                ]
            };

            await Assert.ThrowsAsync<EntityNotFoundException>(() => repository.AddExerciseAsync("2", exercise));
        }
    }
}
