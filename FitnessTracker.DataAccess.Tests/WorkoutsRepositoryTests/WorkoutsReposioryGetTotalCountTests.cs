using FitnessTracker.Application.WorkoutFilters;
using FitnessTracker.Application.WorkoutOrdering;
using FitnessTracker.DataAccess.Repositories;
using FitnessTracker.Entities;
using FitnessTracker.Shared.DTO;
using FitnessTracker.Shared.Enums;
using Moq;

namespace FitnessTracker.DataAccess.Tests.WorkoutsRepositoryTests
{
    public class WorkoutsReposioryGetTotalCountTests
        : WorkoutsRepositoryTestsBase
    {


        [Fact]
        public async Task GetTotalCountByUserAsync_ShouldReturnCount_WhenFilterIsNull()
        {
            using var context = CreateDbContext();

            var filterBuilderMock = new Mock<IWorkoutFilterExpressionBuilder>();

            var repository = new WorkoutsRepository(context, _orderingApplierMock.Object, filterBuilderMock.Object);

            var workout1 = new Workout
            {
                Id = "1",
                UserId = "user1",
                Title = "Leg day",
                Type = WorkoutType.Strength,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = new DateTime(2026, 6, 1).ToUniversalTime(),
                Exercises = [],
                ProgressPhotos = []
            };

            var workout2 = new Workout
            {
                Id = "2",
                UserId = "user1",
                Title = "Leg day",
                Type = WorkoutType.Strength,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = new DateTime(2026, 6, 1).ToUniversalTime(),
                Exercises = [],
                ProgressPhotos = []
            };

            var workout3 = new Workout
            {
                Id = "3",
                UserId = "user2",
                Title = "Leg day",
                Type = WorkoutType.Strength,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = new DateTime(2026, 6, 1).ToUniversalTime(),
                Exercises = [],
                ProgressPhotos = []
            };

            context.Workouts.AddRange(
                workout1,
                workout2,
                workout3
            );
            await context.SaveChangesAsync();

            var count = await repository.GetTotalCountByUserAsync("user1");

            Assert.Equal(2, count);
            filterBuilderMock.Verify(x => x.BuildFilterExpression(
                It.IsAny<IEnumerable<WorkoutFilterDTO>>()),
                Times.Never);
        }

        [Fact]
        public async Task GetTotalCountByUserAsync_ShouldReturnCount_WhenFilterIsNotNull()
        {
            using var context = CreateDbContext();

            var filterBuilderMock = new Mock<IWorkoutFilterExpressionBuilder>();
            filterBuilderMock.Setup(x => x.BuildFilterExpression(It.IsAny<IEnumerable<WorkoutFilterDTO>>()))
                        .Returns(workout => workout.Type == WorkoutType.Strength);

            var repository = new WorkoutsRepository(context, _orderingApplierMock.Object, filterBuilderMock.Object);

            var workout1 = new Workout
            {
                Id = "1",
                UserId = "user1",
                Title = "Leg day",
                Type = WorkoutType.Strength,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = new DateTime(2026, 6, 1).ToUniversalTime(),
                Exercises = [],
                ProgressPhotos = []
            };

            var workout2 = new Workout
            {
                Id = "2",
                UserId = "user1",
                Title = "Leg day",
                Type = WorkoutType.Cardio,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = new DateTime(2026, 6, 1).ToUniversalTime(),
                Exercises = [],
                ProgressPhotos = []
            };

            var workout3 = new Workout
            {
                Id = "3",
                UserId = "user2",
                Title = "Leg day",
                Type = WorkoutType.Strength,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = new DateTime(2026, 6, 1).ToUniversalTime(),
                Exercises = [],
                ProgressPhotos = []
            };

            context.Workouts.AddRange(
                workout1,
                workout2,
                workout3
            );
            await context.SaveChangesAsync();

            var count = await repository.GetTotalCountByUserAsync("user1", new List<WorkoutFilterDTO>());

            Assert.Equal(1, count);
            filterBuilderMock.Verify(x => x.BuildFilterExpression(
                It.IsAny<IEnumerable<WorkoutFilterDTO>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetTotalCountByUserAsync_ShouldZero_WhenUserHasNoWorkouts()
        {
            using var context = CreateDbContext();

            var filterBuilderMock = new Mock<IWorkoutFilterExpressionBuilder>();

            var repository = new WorkoutsRepository(context, _orderingApplierMock.Object, filterBuilderMock.Object);

            var workout1 = new Workout
            {
                Id = "1",
                UserId = "user1",
                Title = "Leg day",
                Type = WorkoutType.Strength,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = new DateTime(2026, 6, 1).ToUniversalTime(),
                Exercises = [],
                ProgressPhotos = []
            };

            var workout2 = new Workout
            {
                Id = "2",
                UserId = "user1",
                Title = "Leg day",
                Type = WorkoutType.Cardio,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = new DateTime(2026, 6, 1).ToUniversalTime(),
                Exercises = [],
                ProgressPhotos = []
            };

            var workout3 = new Workout
            {
                Id = "3",
                UserId = "user2",
                Title = "Leg day",
                Type = WorkoutType.Strength,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = new DateTime(2026, 6, 1).ToUniversalTime(),
                Exercises = [],
                ProgressPhotos = []
            };

            context.Workouts.AddRange(
                workout1,
                workout2,
                workout3
            );
            await context.SaveChangesAsync();

            var count = await repository.GetTotalCountByUserAsync("user4");

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task GetAllByUserAsync_ShouldReturnWorkouts_WhenFilterAndOrdersAreNull()
        {
            using var context = CreateDbContext();

            var filterBuilderMock = new Mock<IWorkoutFilterExpressionBuilder>();
            var orderingApplierMock = new Mock<IWorkoutOrderingApplier>();

            var repository = new WorkoutsRepository(context, orderingApplierMock.Object, filterBuilderMock.Object);

            var workout1 = new Workout
            {
                Id = "1",
                UserId = "user1",
                Title = "Leg day",
                Type = WorkoutType.Strength,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = new DateTime(2026, 6, 1).ToUniversalTime(),
                Exercises = [],
                ProgressPhotos = []
            };

            var workout2 = new Workout
            {
                Id = "2",
                UserId = "user1",
                Title = "Leg day",
                Type = WorkoutType.Strength,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = new DateTime(2026, 6, 1).ToUniversalTime(),
                Exercises = [],
                ProgressPhotos = []
            };

            var workout3 = new Workout
            {
                Id = "3",
                UserId = "user2",
                Title = "Leg day",
                Type = WorkoutType.Strength,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = new DateTime(2026, 6, 1).ToUniversalTime(),
                Exercises = [],
                ProgressPhotos = []
            };

            context.Workouts.AddRange(
                workout1,
                workout2,
                workout3
            );
            await context.SaveChangesAsync();

            var workouts = await repository.GetAllByUserIdAsync("user1");

            Assert.Equal(2, workouts.Count());
            Assert.All(workouts, x =>
            {
                Assert.Equal("user1", x.UserId);
            });
            filterBuilderMock.Verify(
                x => x.BuildFilterExpression(It.IsAny<IEnumerable<WorkoutFilterDTO>>()),
                Times.Never);
            orderingApplierMock.Verify(
                x => x.ApplyOrdering(
                    It.IsAny<IQueryable<Workout>>(),
                    It.IsAny<WorkoutOrderingDTO>()),
                Times.Never);
        }
    }
}
