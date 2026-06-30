using FitnessTracker.DataAccess.Repositories;
using FitnessTracker.Entities;
using FitnessTracker.Shared.Enums;
using FitnessTracker.Shared.Exceptions.NotFound;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.DataAccess.Tests.WorkoutsRepositoryTests
{
    public class WorkoutsRepositoryAddPhotoTests
    : WorkoutsRepositoryTestsBase
    {


        [Fact]
        public async Task AddPhotoAsync_ShouldAddPhoto_WhenWorkoutExists()
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

            var photo = "photo2.jpg";

            await repository.AddPhotoAsync("1", photo);

            var dbWorkout = await context.Workouts
                .FirstOrDefaultAsync(x => x.Id == "1");

            Assert.NotNull(dbWorkout);
            Assert.Equal(2, dbWorkout.ProgressPhotos.Count);
            var photosdb = dbWorkout.ProgressPhotos.OrderBy(x => x).ToList();
            Assert.Equal("photo1.jpg", photosdb[0]);
            Assert.Equal("photo2.jpg", photosdb[1]);
        }

        [Fact]
        public async Task AddPhotoAsync_ShouldThrow_WhenWorkoutNotExists()
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

            var photo = "photo2.png";

            await Assert.ThrowsAsync<EntityNotFoundException>(() => repository.AddPhotoAsync("2", photo));
        }
    }
}
