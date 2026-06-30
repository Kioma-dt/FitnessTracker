using FitnessTracker.Entities;
using FitnessTracker.Shared.DTO;
using FitnessTracker.Shared.Enums;
using FitnessTracker.Shared.Exceptions;
using Moq;
using System.Linq.Expressions;

namespace FitnessTracker.Application.Tests.WorkoutFiltersTests
{
    public class WorkoutFilterExpressionBuilderTests
    {
        [Fact]
        public void BuildFilterExpression_ShouldReturnTrue_WhenNoFiltersProvided()
        {
            var filtersImplementaions = new List<IWorkoutFilter>();

            var filters = new List<WorkoutFilterDTO>();

            var workout = new Workout();

            var builder = new WorkoutFilterExpressionBuilder(filtersImplementaions);

            var expression = builder.BuildFilterExpression(filters);

            var compiledExpression = expression.Compile();

            Assert.True(compiledExpression(workout));
        }


        [Fact]
        public void BuildFilterExpression_ShouldCallFilterImplementation_WhenFilterExists()
        {
            var filterMock = new Mock<IWorkoutFilter>();
            filterMock
                .Setup(x => x.FilterType)
                .Returns(WorkoutFilterType.WorkoutType);
            filterMock
                .Setup(x => x.BuildExpression(
                    It.IsAny<ParameterExpression>(),
                    "Cardio"))
                .Returns(Expression.Constant(true));

            var filtersImplementaions = new List<IWorkoutFilter>() { filterMock.Object };

            var filters = new List<WorkoutFilterDTO>
            {
                new(
                    WorkoutFilterType.WorkoutType,
                    "Cardio")
            };

            var workout = new Workout { Type = WorkoutType.Cardio };

            var builder = new WorkoutFilterExpressionBuilder(filtersImplementaions);

            var expression = builder.BuildFilterExpression(filters);

            var compiledExpression = expression.Compile();

            Assert.True(compiledExpression(workout));
            filterMock.Verify(
                x => x.BuildExpression(
                    It.IsAny<ParameterExpression>(),
                    "Cardio"),
                Times.Once);
        }


        [Fact]
        public void BuildFilterExpression_ShouldRetrunTrue_WhenBothFiltersTrue()
        {
            var filter1 = new Mock<IWorkoutFilter>();
            filter1
                .Setup(x => x.FilterType)
                .Returns(WorkoutFilterType.MinDuration);
            filter1
                .Setup(x => x.BuildExpression(
                    It.IsAny<ParameterExpression>(),
                    It.IsAny<string>()))
                .Returns(Expression.Constant(true));

            var filter2 = new Mock<IWorkoutFilter>();
            filter2
                .Setup(x => x.FilterType)
                .Returns(WorkoutFilterType.MaxDuration);
            filter2
                .Setup(x => x.BuildExpression(
                    It.IsAny<ParameterExpression>(),
                    It.IsAny<string>()))
                .Returns(Expression.Constant(true));
            var filtersImplementaions = new List<IWorkoutFilter>() { filter1.Object, filter2.Object };

            var filters = new List<WorkoutFilterDTO>
            {
                new(
                    WorkoutFilterType.MinDuration,
                    "10"),

                new(
                    WorkoutFilterType.MaxDuration,
                    "20")
            };


            var workout = new Workout { Duration = new TimeSpan(15) };

            var builder = new WorkoutFilterExpressionBuilder(filtersImplementaions);

            var expression = builder.BuildFilterExpression(filters);

            var compiledExpression = expression.Compile();

            Assert.True(compiledExpression(workout));
            filter1.Verify(
                x => x.BuildExpression(
                    It.IsAny<ParameterExpression>(),
                    "10"),
                Times.Once);
            filter2.Verify(
                x => x.BuildExpression(
                    It.IsAny<ParameterExpression>(),
                    "20"),
                Times.Once);
        }

        [Fact]
        public void BuildFilterExpression_ShouldRetrunFalse_WhenOneFilterIsFalse()
        {
            var filter1 = new Mock<IWorkoutFilter>();
            filter1
                .Setup(x => x.FilterType)
                .Returns(WorkoutFilterType.MinDuration);
            filter1
                .Setup(x => x.BuildExpression(
                    It.IsAny<ParameterExpression>(),
                    It.IsAny<string>()))
                .Returns(Expression.Constant(true));

            var filter2 = new Mock<IWorkoutFilter>();
            filter2
                .Setup(x => x.FilterType)
                .Returns(WorkoutFilterType.MaxDuration);
            filter2
                .Setup(x => x.BuildExpression(
                    It.IsAny<ParameterExpression>(),
                    It.IsAny<string>()))
                .Returns(Expression.Constant(false));
            var filtersImplementaions = new List<IWorkoutFilter>() { filter1.Object, filter2.Object };

            var filters = new List<WorkoutFilterDTO>
            {
                new(
                    WorkoutFilterType.MinDuration,
                    "10"),

                new(
                    WorkoutFilterType.MaxDuration,
                    "20")
            };


            var workout = new Workout { Duration = new TimeSpan(40) };

            var builder = new WorkoutFilterExpressionBuilder(filtersImplementaions);

            var expression = builder.BuildFilterExpression(filters);

            var compiledExpression = expression.Compile();

            Assert.False(compiledExpression(workout));
            filter1.Verify(
                x => x.BuildExpression(
                    It.IsAny<ParameterExpression>(),
                    "10"),
                Times.Once);
            filter2.Verify(
                x => x.BuildExpression(
                    It.IsAny<ParameterExpression>(),
                    "20"),
                Times.Once);
        }


        [Fact]
        public void BuildFilterExpression_ShouldThrow_WhenFilterImplementationDoesNotExist()
        {
            var filter = new Mock<IWorkoutFilter>();
            filter
                .Setup(x => x.FilterType)
                .Returns(WorkoutFilterType.MinDuration);
            var filtersImplementaions = new List<IWorkoutFilter>() { filter.Object };

            var filters = new List<WorkoutFilterDTO>
            {
                new(
                    WorkoutFilterType.MaxDuration,
                    "20")
            };

            var workout = new Workout { Duration = new TimeSpan(40) };

            var builder = new WorkoutFilterExpressionBuilder(filtersImplementaions);

            Assert.Throws<NotImplementedFunctionalityException>(
                () => builder.BuildFilterExpression(filters));
        }
    }
}
