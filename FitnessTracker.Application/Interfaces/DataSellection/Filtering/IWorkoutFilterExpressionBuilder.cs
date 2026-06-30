using FitnessTracker.Shared.DTO.Application.Workout;
using System.Linq.Expressions;

namespace FitnessTracker.Application.Interfaces.DataSellection.Filtering
{
    public interface IWorkoutFilterExpressionBuilder
    {
        Expression<Func<Workout, bool>> BuildFilterExpression(IEnumerable<WorkoutFilterDTO> filters);
    }
}
