using System.Linq.Expressions;

namespace FitnessTracker.Inrastructure.DataSellection.Ordering.WorkoutOrders
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
        public WorkoutOrderingType WorkoutOrderingType { get; } = WorkoutOrderingType.CaloriesBurned;

        public MemberExpression BuildExpression(ParameterExpression parameter)
        {
            var property = Expression.Property(
                parameter,
                nameof(Workout.CaloriesBurned));

            return property;
        }
    }
}
