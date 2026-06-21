using FitnessTracker.Shared.DTO;
using FitnessTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FitnessTracker.Application.WorkoutOrdering
{
    public interface IWorkoutOrderingApplier
    {
        IQueryable<Workout> ApplyOrdering(IQueryable<Workout> query, 
            WorkoutOrderingDTO ordering);
    }
    public class WorkoutOrderingApplier
        : IWorkoutOrderingApplier
    {
        private readonly Dictionary<WorkoutOrderingType, IWorkoutOrder> _orders;

        public WorkoutOrderingApplier(IEnumerable<IWorkoutOrder> orders)
        {
            _orders = orders.ToDictionary(x => x.WorkoutOrderingType);
        }
        public IQueryable<Workout> ApplyOrdering(IQueryable<Workout> query,
            WorkoutOrderingDTO ordering)
        {
                var parameter = Expression.Parameter(typeof(Workout), "workout");

                if(!_orders.TryGetValue(ordering.OrderBy, out var order))
                {
                    throw new NotImplementedFunctionalityException($"No order implementation found for order type: {ordering.OrderBy}");
                }

                var property = order.BuildExpression(parameter);

                var selector = Expression.Lambda(property, parameter);

                bool desc = ordering.Descending;

                var methodName = desc
                    ? nameof(Queryable.OrderByDescending)
                    : nameof(Queryable.OrderBy);

                var call = Expression.Call(
                    typeof(Queryable),
                    methodName,
                    new[]
                    {
                        typeof(Workout),
                        property.Type
                    },
                    query.Expression,
                    Expression.Quote(selector)
                 );

                var result = query.Provider.CreateQuery<Workout>(call);

                return ((IOrderedQueryable<Workout>)result)
                    .ThenBy(x => x.Id);
        }
    }
}
