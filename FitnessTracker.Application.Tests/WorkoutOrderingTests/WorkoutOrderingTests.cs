using FitnessTracker.Application.WorkoutOrdering;
using FitnessTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FitnessTracker.Application.Tests.WorkoutOrderingTests
{
    public class WorkoutOrderingTests
    {
        [Fact]
        public void DateBuildExpression_ShouldReturnWorkoutDateProperty()
        {
            var order = new WorkoutOrderByDate();

            var parameter = Expression.Parameter(typeof(Workout), "workout");

            var expression = order.BuildExpression(parameter);

            Assert.Equal(nameof(Workout.WorkoutDate), expression.Member.Name);
            Assert.Same(parameter, expression.Expression);
        }

        [Fact]
        public void BuildExpression_ShouldReturnCaloriesBurnedProperty()
        {
            var order = new WorkoutOrderByBurnedCalories();

            var parameter = Expression.Parameter(typeof(Workout), "workout");

            var expression = order.BuildExpression(parameter);

            Assert.Equal(nameof(Workout.CaloriesBurned), expression.Member.Name);
            Assert.Same(parameter, expression.Expression);
        }
    }
}
