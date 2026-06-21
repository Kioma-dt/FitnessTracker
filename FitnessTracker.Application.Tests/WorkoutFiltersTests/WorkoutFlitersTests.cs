using FitnessTracker.Application.WorkoutFilters;
using FitnessTracker.Entities;
using FitnessTracker.Shared.Enums;
using FitnessTracker.Shared.Exceptions;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace FitnessTracker.Application.Tests.WorkoutFiltersTests
{
    public class WorkoutFlitersTests
    {
        [Fact]
        public void FromDateBuildExpression_ShouldReturnTrue_WhenWorkoutDateIsAfterFromDate()
        {
            var fromDate = new DateTime(2026, 05, 21);

            var workout = new Workout { WorkoutDate = new DateTime(2026, 06, 21) };

            var filter = new FromDateWorkoutFilter();

            var parameter = Expression.Parameter(typeof(Workout));
            var expression = filter.BuildExpression(parameter, fromDate.ToString());

            var compiledExpression = CompileExpression(parameter, expression);
            Assert.True(compiledExpression(workout));
        }

        [Fact]
        public void FromDateBuildExpression_ShouldReturnFalse_WhenWorkoutDateIsBeforeFromDate()
        {
            var fromDate = new DateTime(2026, 07, 21);

            var workout = new Workout { WorkoutDate = new DateTime(2026, 06, 21) };

            var filter = new FromDateWorkoutFilter();

            var parameter = Expression.Parameter(typeof(Workout));
            var expression = filter.BuildExpression(parameter, fromDate.ToString());

            var compiledExpression = CompileExpression(parameter, expression);
            Assert.False(compiledExpression(workout));
        }

        [Fact]
        public void FromDateBuildExpression_ShouldThrow_WhenValueIsInvalid()
        {
            var filter = new FromDateWorkoutFilter();
            Assert.Throws<WrongFilterValueFormatException>(() =>
                filter.BuildExpression(
                    Expression.Parameter(typeof(Workout)),
                    "abc"));
        }

        [Fact]
        public void ToDateBuildExpression_ShouldReturnTrue_WhenWorkoutDateIsBeforeToDate()
        {
            var toDate = new DateTime(2026, 07, 21);

            var workout = new Workout { WorkoutDate = new DateTime(2026, 06, 21) };

            var filter = new ToDateWorkoutFilter();

            var parameter = Expression.Parameter(typeof(Workout));
            var expression = filter.BuildExpression(parameter, toDate.ToString());

            var compiledExpression = CompileExpression(parameter, expression);
            Assert.True(compiledExpression(workout));
        }

        [Fact]
        public void ToDateBuildExpression_ShouldReturnFalse_WhenWorkoutDateIsAfterToDate()
        {
            var toDate = new DateTime(2026, 05, 21);

            var workout = new Workout { WorkoutDate = new DateTime(2026, 06, 21) };

            var filter = new ToDateWorkoutFilter();

            var parameter = Expression.Parameter(typeof(Workout));
            var expression = filter.BuildExpression(parameter, toDate.ToString());

            var compiledExpression = CompileExpression(parameter, expression);
            Assert.False(compiledExpression(workout));
        }

        [Fact]
        public void ToDateBuildExpression_ShouldThrow_WhenValueIsInvalid()
        {
            var filter = new ToDateWorkoutFilter();
            Assert.Throws<WrongFilterValueFormatException>(() =>
                filter.BuildExpression(
                    Expression.Parameter(typeof(Workout)),
                    "abc"));
        }

        [Fact]
        public void MinDurationBuildExpression_ShouldReturnTrue_WhenWorkoutDurationIsGreaterMinDuration()
        {
            var minDuration = new TimeSpan(67);

            var workout = new Workout { Duration = new TimeSpan(70) };

            var filter = new MinDurationWorkoutFilter();

            var parameter = Expression.Parameter(typeof(Workout));
            var expression = filter.BuildExpression(parameter, minDuration.ToString());

            var compiledExpression = CompileExpression(parameter, expression);
            Assert.True(compiledExpression(workout));
        }

        [Fact]
        public void MinDurationBuildExpression_ShouldReturnFalse_WhenWorkoutDurationIsLessMinDuration()
        {
            var minDuration = new TimeSpan(67);

            var workout = new Workout { Duration = new TimeSpan(65) };

            var filter = new MinDurationWorkoutFilter();

            var parameter = Expression.Parameter(typeof(Workout));
            var expression = filter.BuildExpression(parameter, minDuration.ToString());

            var compiledExpression = CompileExpression(parameter, expression);
            Assert.False(compiledExpression(workout));
        }

        [Fact]
        public void MinDurationBuildExpression_ShouldThrow_WhenValueIsInvalid()
        {
            var filter = new MinDurationWorkoutFilter();
            Assert.Throws<WrongFilterValueFormatException>(() =>
                filter.BuildExpression(
                    Expression.Parameter(typeof(Workout)),
                    "abc"));
        }

        [Fact]
        public void MaxDurationBuildExpression_ShouldReturnTrue_WhenWorkoutDurationIsLessMaxDuration()
        {
            var maxDuration = new TimeSpan(67);

            var workout = new Workout { Duration = new TimeSpan(65) };

            var filter = new MaxDurationWorkoutFilter();

            var parameter = Expression.Parameter(typeof(Workout));
            var expression = filter.BuildExpression(parameter, maxDuration.ToString());

            var compiledExpression = CompileExpression(parameter, expression);
            Assert.True(compiledExpression(workout));
        }

        [Fact]
        public void MaxDurationBuildExpression_ShouldReturnFalse_WhenWorkoutDurationIsGreaterMaxDuration()
        {
            var maxDuration = new TimeSpan(67);

            var workout = new Workout { Duration = new TimeSpan(70) };

            var filter = new MaxDurationWorkoutFilter();

            var parameter = Expression.Parameter(typeof(Workout));
            var expression = filter.BuildExpression(parameter, maxDuration.ToString());

            var compiledExpression = CompileExpression(parameter, expression);
            Assert.False(compiledExpression(workout));
        }

        [Fact]
        public void MaxDurationBuildExpression_ShouldThrow_WhenValueIsInvalid()
        {
            var filter = new MaxDurationWorkoutFilter();
            Assert.Throws<WrongFilterValueFormatException>(() =>
                filter.BuildExpression(
                    Expression.Parameter(typeof(Workout)),
                    "abc"));
        }

        [Fact]
        public void WorkoutTypeBuildExpression_ShouldReturnTrue_WhenWorkoutTypeMatches()
        {
            WorkoutType type = WorkoutType.Strength;

            var workout = new Workout { Type = WorkoutType.Strength };

            var filter = new TypeWorkoutFilter();

            var parameter = Expression.Parameter(typeof(Workout));
            var expression = filter.BuildExpression(parameter, type.ToString());

            var compiledExpression = CompileExpression(parameter, expression);
            Assert.True(compiledExpression(workout));
        }

        [Fact]
        public void WorkoutTypeBuildExpression_ShouldReturnFalse_WhenWorkoutTypeNotMatches()
        {
            WorkoutType type = WorkoutType.Strength;

            var workout = new Workout { Type = WorkoutType.Cardio };

            var filter = new TypeWorkoutFilter();

            var parameter = Expression.Parameter(typeof(Workout));
            var expression = filter.BuildExpression(parameter, type.ToString());

            var compiledExpression = CompileExpression(parameter, expression);
            Assert.False(compiledExpression(workout));
        }

        [Fact]
        public void WorkoutTypeBuildExpression_ShouldThrow_WhenValueIsInvalid()
        {
            var filter = new TypeWorkoutFilter();
            Assert.Throws<WrongFilterValueFormatException>(() =>
                filter.BuildExpression(
                    Expression.Parameter(typeof(Workout)),
                    "abc"));
        }

        private Func<Workout, bool> CompileExpression(ParameterExpression parameter,Expression expression)
        {
            return Expression
                    .Lambda<Func<Workout, bool>>(expression, parameter)
                    .Compile();
        }
    }
}
