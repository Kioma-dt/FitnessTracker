using FitnessTracker.Application.WorkoutFilters;
using FitnessTracker.Application.WorkoutOrdering;
using FitnessTracker.DataAccess.Repositories;
using FitnessTracker.Entities;
using FitnessTracker.Shared.Enums;
using FitnessTracker.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FitnessTracker.DataAccess.Tests
{
    public class WorkoutsRepositoryTests
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


        private FitnessTrackerDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<FitnessTrackerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new FitnessTrackerDbContext(options);
        }

        private readonly Mock<IWorkoutOrderingApplier> _orderingApplierMock;
        private readonly Mock<IWorkoutFilterExpressionBuilder> _filterBuilderMock;

        public WorkoutsRepositoryTests()
        {
            _orderingApplierMock = new Mock<IWorkoutOrderingApplier>();
            _filterBuilderMock = new Mock<IWorkoutFilterExpressionBuilder>();
        }
    }
}
