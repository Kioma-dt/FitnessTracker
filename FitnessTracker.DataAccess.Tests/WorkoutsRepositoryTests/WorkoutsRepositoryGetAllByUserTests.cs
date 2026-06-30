using FitnessTracker.DataAccess.Repositories;
using FitnessTracker.Entities;
using FitnessTracker.Shared.DTO;
using FitnessTracker.Shared.Enums;
using FitnessTracker.Shared.Exceptions;
using Moq;

namespace FitnessTracker.DataAccess.Tests.WorkoutsRepositoryTests
{
    public class WorkoutsRepositoryGetAllByUserTests
        : WorkoutsRepositoryTestsBase
    {
        [Fact]
        public async Task GetAllByUserAsync_ShouldReturnWorkouts_WhenFilterIsNoNull()
        {
            using var context = CreateDbContext();

            var filterBuilderMock = new Mock<IWorkoutFilterExpressionBuilder>();
            filterBuilderMock.Setup(x => x.BuildFilterExpression(
                It.IsAny<IEnumerable<WorkoutFilterDTO>>()))
                .Returns(x => x.Type == WorkoutType.Strength);
            var orderingApplierMock = new Mock<IWorkoutOrderingApplier>();

            var repository = new WorkoutsRepository(context, orderingApplierMock.Object, filterBuilderMock.Object);

            var workout1 = new Workout
            {
                Id = "1",
                UserId = "user1",
                Title = "Leg day",
                Type = WorkoutType.Cardio,
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

            var workouts = await repository.GetAllByUserIdAsync("user1", 1, 10, new List<WorkoutFilterDTO>());

            Assert.Single(workouts);
            Assert.Equal(WorkoutType.Strength, workouts.First().Type);
            filterBuilderMock.Verify(
                x => x.BuildFilterExpression(It.IsAny<IEnumerable<WorkoutFilterDTO>>()),
                Times.Once);
            orderingApplierMock.Verify(
                x => x.ApplyOrdering(
                    It.IsAny<IQueryable<Workout>>(),
                    It.IsAny<WorkoutOrderingDTO>()),
                Times.Never);
        }

        [Fact]
        public async Task GetAllByUserAsync_ShouldReturnWorkouts_WhenOrderIsNotNull()
        {
            using var context = CreateDbContext();

            var filterBuilderMock = new Mock<IWorkoutFilterExpressionBuilder>();
            var orderingApplierMock = new Mock<IWorkoutOrderingApplier>();

            orderingApplierMock
                .Setup(x => x.ApplyOrdering(
                    It.IsAny<IQueryable<Workout>>(),
                    It.IsAny<WorkoutOrderingDTO>()))
                .Returns((IQueryable<Workout> query, WorkoutOrderingDTO ordering) =>
                    query.OrderBy(x => x.Title));

            var repository = new WorkoutsRepository(context, orderingApplierMock.Object, filterBuilderMock.Object);

            var workout1 = new Workout
            {
                Id = "1",
                UserId = "user1",
                Title = "Leg day",
                Type = WorkoutType.Cardio,
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
                Title = "ALeg day",
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

            var workouts = await repository.GetAllByUserIdAsync("user1", 1, 10, null, new WorkoutOrderingDTO(WorkoutOrderingType.Date, false));

            Assert.Equal(2, workouts.Count());
            Assert.Equal("2", workouts.First().Id);
            filterBuilderMock.Verify(
                x => x.BuildFilterExpression(It.IsAny<IEnumerable<WorkoutFilterDTO>>()),
                Times.Never);
            orderingApplierMock.Verify(
                x => x.ApplyOrdering(
                    It.IsAny<IQueryable<Workout>>(),
                    It.IsAny<WorkoutOrderingDTO>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAllByUserAsync_ShouldReturnWorkoutsSortedByOd_WhenOrderIsNull()
        {
            using var context = CreateDbContext();

            var filterBuilderMock = new Mock<IWorkoutFilterExpressionBuilder>();
            var orderingApplierMock = new Mock<IWorkoutOrderingApplier>();

            var repository = new WorkoutsRepository(context, orderingApplierMock.Object, filterBuilderMock.Object);

            var workout1 = new Workout
            {
                Id = "3",
                UserId = "user1",
                Title = "Leg day",
                Type = WorkoutType.Cardio,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = new DateTime(2026, 6, 1).ToUniversalTime(),
                Exercises = [],
                ProgressPhotos = []
            };

            var workout2 = new Workout
            {
                Id = "1",
                UserId = "user1",
                Title = "ALeg day",
                Type = WorkoutType.Strength,
                Duration = TimeSpan.FromMinutes(60),
                CaloriesBurned = 500,
                WorkoutDate = new DateTime(2026, 6, 1).ToUniversalTime(),
                Exercises = [],
                ProgressPhotos = []
            };

            var workout3 = new Workout
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

            context.Workouts.AddRange(
                workout1,
                workout2,
                workout3
            );
            await context.SaveChangesAsync();

            var workouts = await repository.GetAllByUserIdAsync("user1");

            Assert.Equal(3, workouts.Count());
            Assert.Equal("1", workouts.ElementAt(0).Id);
            Assert.Equal("2", workouts.ElementAt(1).Id);
            Assert.Equal("3", workouts.ElementAt(2).Id);
            filterBuilderMock.Verify(
                x => x.BuildFilterExpression(It.IsAny<IEnumerable<WorkoutFilterDTO>>()),
                Times.Never);
            orderingApplierMock.Verify(
                x => x.ApplyOrdering(
                    It.IsAny<IQueryable<Workout>>(),
                    It.IsAny<WorkoutOrderingDTO>()),
                Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-5)]
        public async Task GetAllByUserAsync_ShouldThrow_WhenPageIsLessOrEqualZero(int page)
        {
            using var context = CreateDbContext();

            var filterBuilderMock = new Mock<IWorkoutFilterExpressionBuilder>();
            var orderingApplierMock = new Mock<IWorkoutOrderingApplier>();

            var repository = new WorkoutsRepository(context, orderingApplierMock.Object, filterBuilderMock.Object);
            await Assert.ThrowsAsync<WrongWorkoutPageFormat>(() =>  repository.GetAllByUserIdAsync("user1", page));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-5)]
        public async Task GetAllByUserAsync_ShouldThrow_WhenPageSizeIsLessOrEqualZero(int pageSize)
        {
            using var context = CreateDbContext();

            var filterBuilderMock = new Mock<IWorkoutFilterExpressionBuilder>();
            var orderingApplierMock = new Mock<IWorkoutOrderingApplier>();

            var repository = new WorkoutsRepository(context, orderingApplierMock.Object, filterBuilderMock.Object);
            await Assert.ThrowsAsync<WrongWorkoutPageFormat>(() => repository.GetAllByUserIdAsync("user1", 1, pageSize));
        }

        [Fact]
        public async Task GetAllByUserAsync_ShouldReturnPagedWorkouts_WhenPageValuesProvided()
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
                Type = WorkoutType.Cardio,
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
                Title = "ALeg day",
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
                UserId = "user1",
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

            var workouts = await repository.GetAllByUserIdAsync("user1", 2, 2);

            Assert.Single(workouts);
            Assert.Equal("3", workouts.ElementAt(0).Id);
        }
    }
}
