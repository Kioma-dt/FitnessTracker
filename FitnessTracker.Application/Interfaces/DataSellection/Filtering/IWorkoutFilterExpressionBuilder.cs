using FitnessTracker.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FitnessTracker.Application.Interfaces.DataSellection.Filtering
{
    public interface IWorkoutFilterExpressionBuilder
    {
        Expression<Func<Workout, bool>> BuildFilterExpression(IEnumerable<WorkoutFilterDTO> filters);
    }
}
