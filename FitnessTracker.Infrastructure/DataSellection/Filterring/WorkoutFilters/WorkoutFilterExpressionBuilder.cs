using FitnessTracker.Application.Interfaces.DataSellection.Filtering;
using FitnessTracker.Shared.DTO.Application.Workout;
using System.Linq.Expressions;


namespace FitnessTracker.Inrastructure.DataSellection.Filterring.WorkoutFilters
{
    public class WorkoutFilterExpressionBuilder 
        : IWorkoutFilterExpressionBuilder
    {
        private readonly Dictionary<WorkoutFilterType, IWorkoutFilter> _filters;
        public WorkoutFilterExpressionBuilder(IEnumerable<IWorkoutFilter> filters)
        {
            _filters = filters.ToDictionary(x => x.FilterType);
        }
        public Expression<Func<Workout, bool>> BuildFilterExpression(IEnumerable<WorkoutFilterDTO> filters)
        {
            var parameter = Expression.Parameter(typeof(Workout), "workout");
            Expression? combinedExpression = null;

            foreach (var filter in filters)
            {
                if(!_filters.TryGetValue(filter.FilterType, out var filterImplementation))
                {
                    throw new NotImplementedFunctionalityException($"No filter implementation found for filter type {filter.FilterType}");
                }

                var filterExpression = filterImplementation.BuildExpression(parameter, filter.FilterValue);
                combinedExpression = combinedExpression == null ? filterExpression : Expression.AndAlso(combinedExpression, filterExpression);
            }

            if (combinedExpression == null)
            {
                combinedExpression = Expression.Constant(true);
            }

            return Expression.Lambda<Func<Workout, bool>>(combinedExpression, parameter);
        }
    }
}
