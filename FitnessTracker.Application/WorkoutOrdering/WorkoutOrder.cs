using FitnessTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FitnessTracker.Application.WorkoutOrdering
{
    public interface IWorkoutOrder
    {
        WorkoutOrderingType WorkoutOrderingType { get; }
        MemberExpression BuildExpression(ParameterExpression parameter);
    }

    public class WorkoutOrderByDate
        : IWorkoutOrder
    {
        public WorkoutOrderingType WorkoutOrderingType { get; } = WorkoutOrderingType.Date;

        public MemberExpression BuildExpression(ParameterExpression parameter)
        {
            var property = Expression.Property(
                parameter,
                nameof(Workout.WorkoutDate));

            return property;
        }
    }

    public class WorkoutOrderByBurnedCalories
        : IWorkoutOrder
    {
        public WorkoutOrderingType WorkoutOrderingType { get; } = WorkoutOrderingType.BurnedCalories;

        public MemberExpression BuildExpression(ParameterExpression parameter)
        {
            var property = Expression.Property(
                parameter,
                nameof(Workout.CaloriesBurned));

            return property;
        }
    }
}
