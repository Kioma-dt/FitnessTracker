using FitnessTracker.Application.WorkoutFilters;
using FitnessTracker.Application.WorkoutOrdering;
using FitnessTracker.DataAccess.Repositories;
using FitnessTracker.Entities;
using FitnessTracker.Shared.DTO;
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
