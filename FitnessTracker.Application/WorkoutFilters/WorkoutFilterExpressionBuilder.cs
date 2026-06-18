using FitnessTracker.Shared.DTO;
using FitnessTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FitnessTracker.Application.WorkoutFilters
{
    public interface IWorkoutFilterExpressionBuilder
    {
        Expression<Func<Workout, bool>> BuildFilterExpression(IEnumerable<WorkoutFilterDTO> filters); 
    }

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
                if (filter.FilterValue is not null)
                {
                    var filterExpression = filterImplementation.BuildExpression(parameter, filter.FilterValue ?? string.Empty);
                    combinedExpression = combinedExpression == null ? filterExpression : Expression.AndAlso(combinedExpression, filterExpression);
                }
            }
            if (combinedExpression == null)
            {
                combinedExpression = Expression.Constant(true);
            }
            return Expression.Lambda<Func<Workout, bool>>(combinedExpression, parameter);
        }
    }
}
