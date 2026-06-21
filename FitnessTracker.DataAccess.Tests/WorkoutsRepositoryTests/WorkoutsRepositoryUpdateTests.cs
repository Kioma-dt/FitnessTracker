using FitnessTracker.DataAccess.Repositories;
using FitnessTracker.Entities;
using FitnessTracker.Shared.DTO;
using FitnessTracker.Shared.Enums;
using FitnessTracker.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.DataAccess.Tests.WorkoutsRepositoryTests
{
    public class WorkoutsRepositoryUpdateTests
    : WorkoutsRepositoryTestsBase
    {

        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenWorkoutNotExists()
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
                WorkoutDate = new DateTime(2026, 6, 1).ToUniversalTime(),
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

            var dto = new WorkoutUpdateDTO
            {
                Title = "Cardio",
                Type = WorkoutType.Cardio,
                Duration = TimeSpan.FromMinutes(30),
                CaloriesBurned = 250,
                WorkoutDate = new DateTime(2026, 5, 1).ToUniversalTime(),
                Exercises =
                [
                    new ExerciseUpdateDTO(
                        "Bench press",
                        [
                            new SetUpdateDTO(80, 12),
                            new SetUpdateDTO(90, 10)
                        ])
                ],
                ProgressPhotos =
                [
                    "image1.png",
                    "image2.png"
                ]
            };

            await Assert.ThrowsAsync<EntityNotFoundException>(() => repository.UpdateAsync("2", dto));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateScalarFields_WhenScalarFieldsProvided()
        {
            using var context = CreateDbContext();

            var repository = new WorkoutsRepository(context, _orderingApplierMock.Object, _filterBuilderMock.Object);

            var workout = new Workout
            {
                Id = "1",
                UserId = "user1",
                Title = "Leg day",
                Type = WorkoutType.Strength,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = new DateTime(2026, 6, 1).ToUniversalTime(),
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

            var dto = new WorkoutUpdateDTO
            {
                Title = "Cardio",
                Type = WorkoutType.Cardio,
                Duration = TimeSpan.FromMinutes(30),
                CaloriesBurned = 250,
                WorkoutDate = new DateTime(2026, 5, 1).ToUniversalTime(),
                Exercises =
                [
                    new ExerciseUpdateDTO(
                        "Bench press",
                        [
                            new SetUpdateDTO(80, 12),
                            new SetUpdateDTO(90, 10)
                        ])
                ],
                ProgressPhotos =
                [
                    "image1.png",
                    "image2.png"
                ]
            };

            await repository.UpdateAsync("1", dto);

            var workoutUpdated = await context.Workouts.FirstOrDefaultAsync(x => x.Id == "1");

            Assert.NotNull(workoutUpdated);
            Assert.Equal("Cardio", workoutUpdated.Title);
            Assert.Equal(WorkoutType.Cardio, workoutUpdated.Type);
            Assert.Equal(TimeSpan.FromMinutes(30), workoutUpdated.Duration);
            Assert.Equal(250, workoutUpdated.CaloriesBurned);
            Assert.Equal(new DateTime(2026, 5, 1).ToUniversalTime(), workoutUpdated.WorkoutDate);
        }

        [Fact]
        public async Task UpdateAsync_ShouldNotUpdateScalarFields_WhenScalarFieldsNotProvided()
        {
            using var context = CreateDbContext();

            var repository = new WorkoutsRepository(context, _orderingApplierMock.Object, _filterBuilderMock.Object);

            var workout = new Workout
            {
                Id = "1",
                UserId = "user1",
                Title = "Leg day",
                Type = WorkoutType.Strength,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = new DateTime(2026, 6, 1).ToUniversalTime(),
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

            var dto = new WorkoutUpdateDTO
            {
                Exercises =
                [
                    new ExerciseUpdateDTO(
                        "Bench press",
                        [
                            new SetUpdateDTO(80, 12),
                            new SetUpdateDTO(90, 10)
                        ])
                ],
                ProgressPhotos =
                [
                    "image1.png",
                    "image2.png"
                ]
            };

            await repository.UpdateAsync("1", dto);

            var workoutUpdated = await context.Workouts.FirstOrDefaultAsync(x => x.Id == "1");

            Assert.NotNull(workoutUpdated);
            Assert.Equal("Leg day", workoutUpdated.Title);
            Assert.Equal(WorkoutType.Strength, workoutUpdated.Type);
            Assert.Equal(TimeSpan.FromMinutes(60), workoutUpdated.Duration);
            Assert.Equal(500, workoutUpdated.CaloriesBurned);
            Assert.Equal(new DateTime(2026, 6, 1).ToUniversalTime(), workoutUpdated.WorkoutDate);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateExercises_WhenExercisesProvided()
        {
            using var context = CreateDbContext();

            var repository = new WorkoutsRepository(context, _orderingApplierMock.Object, _filterBuilderMock.Object);

            var workout = new Workout
            {
                Id = "1",
                UserId = "user1",
                Title = "Leg day",
                Type = WorkoutType.Strength,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = new DateTime(2026, 6, 1).ToUniversalTime(),
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

            var dto = new WorkoutUpdateDTO
            {
                Title = "Cardio",
                Type = WorkoutType.Cardio,
                Duration = TimeSpan.FromMinutes(30),
                CaloriesBurned = 250,
                WorkoutDate = new DateTime(2026, 5, 1).ToUniversalTime(),
                Exercises =
                [
                    new ExerciseUpdateDTO(
                        "Bench press",
                        [
                            new SetUpdateDTO(80, 12),
                            new SetUpdateDTO(90, 10)
                        ]),
                    new ExerciseUpdateDTO(
                        "Jogging",
                        [
                            new SetUpdateDTO(1, 1),
                        ])
                ],
                ProgressPhotos =
                [
                    "image1.png",
                    "image2.png"
                ]
            };

            await repository.UpdateAsync("1", dto);

            var workoutUpdated = await context.Workouts.FirstOrDefaultAsync(x => x.Id == "1");

            Assert.NotNull(workoutUpdated);
            Assert.Equal(2, workoutUpdated.Exercises.Count);
            Assert.Equal("Bench press", workoutUpdated.Exercises[0].Name);
            Assert.Equal(2, workoutUpdated.Exercises[0].Sets.Count);
            Assert.Equal("Jogging", workoutUpdated.Exercises[1].Name);
            Assert.Single(workoutUpdated.Exercises[1].Sets);
        }

        [Fact]
        public async Task UpdateAsync_ShouldNotUpdateExercises_WhenExercisesNotProvided()
        {
            using var context = CreateDbContext();

            var repository = new WorkoutsRepository(context, _orderingApplierMock.Object, _filterBuilderMock.Object);

            var workout = new Workout
            {
                Id = "1",
                UserId = "user1",
                Title = "Leg day",
                Type = WorkoutType.Strength,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = new DateTime(2026, 6, 1).ToUniversalTime(),
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

            var dto = new WorkoutUpdateDTO
            {
                Title = "Cardio",
                Type = WorkoutType.Cardio,
                Duration = TimeSpan.FromMinutes(30),
                CaloriesBurned = 250,
                WorkoutDate = new DateTime(2026, 5, 1).ToUniversalTime(),
                ProgressPhotos =
                [
                    "image1.png",
                    "image2.png"
                ]
            };

            await repository.UpdateAsync("1", dto);

            var workoutUpdated = await context.Workouts.FirstOrDefaultAsync(x => x.Id == "1");

            Assert.NotNull(workoutUpdated);
            Assert.Single(workoutUpdated.Exercises);
            Assert.Equal("Squat", workoutUpdated.Exercises[0].Name);
            Assert.Equal(2, workoutUpdated.Exercises[0].Sets.Count);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdatePhotos_WhenPhotosProvided()
        {
            using var context = CreateDbContext();

            var repository = new WorkoutsRepository(context, _orderingApplierMock.Object, _filterBuilderMock.Object);

            var workout = new Workout
            {
                Id = "1",
                UserId = "user1",
                Title = "Leg day",
                Type = WorkoutType.Strength,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = new DateTime(2026, 6, 1).ToUniversalTime(),
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

            var dto = new WorkoutUpdateDTO
            {
                Title = "Cardio",
                Type = WorkoutType.Cardio,
                Duration = TimeSpan.FromMinutes(30),
                CaloriesBurned = 250,
                WorkoutDate = new DateTime(2026, 5, 1).ToUniversalTime(),
                Exercises =
                [
                    new ExerciseUpdateDTO(
                        "Bench press",
                        [
                            new SetUpdateDTO(80, 12),
                            new SetUpdateDTO(90, 10)
                        ]),
                    new ExerciseUpdateDTO(
                        "Jogging",
                        [
                            new SetUpdateDTO(1, 1),
                        ])
                ],
                ProgressPhotos =
                [
                    "image1.png",
                    "image2.png"
                ]
            };

            await repository.UpdateAsync("1", dto);

            var workoutUpdated = await context.Workouts.FirstOrDefaultAsync(x => x.Id == "1");

            Assert.NotNull(workoutUpdated);
            Assert.Equal(2, workoutUpdated.ProgressPhotos.Count);
            Assert.Equal("image1.png", workoutUpdated.ProgressPhotos[0]);
            Assert.Equal("image2.png", workoutUpdated.ProgressPhotos[1]);
        }

        [Fact]
        public async Task UpdateAsync_ShouldNotUpdatePhotos_WhenPhotosNotProvided()
        {
            using var context = CreateDbContext();

            var repository = new WorkoutsRepository(context, _orderingApplierMock.Object, _filterBuilderMock.Object);

            var workout = new Workout
            {
                Id = "1",
                UserId = "user1",
                Title = "Leg day",
                Type = WorkoutType.Strength,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = new DateTime(2026, 6, 1).ToUniversalTime(),
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

            var dto = new WorkoutUpdateDTO
            {
                Title = "Cardio",
                Type = WorkoutType.Cardio,
                Duration = TimeSpan.FromMinutes(30),
                CaloriesBurned = 250,
                WorkoutDate = new DateTime(2026, 5, 1).ToUniversalTime(),
            };

            await repository.UpdateAsync("1", dto);

            var workoutUpdated = await context.Workouts.FirstOrDefaultAsync(x => x.Id == "1");

            Assert.NotNull(workoutUpdated);
            Assert.Single(workoutUpdated.ProgressPhotos);
            Assert.Equal("photo1.jpg", workoutUpdated.ProgressPhotos[0]);
        }
    }
}
