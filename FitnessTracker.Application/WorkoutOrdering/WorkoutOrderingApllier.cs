using FitnessTracker.Shared.DTO;
using FitnessTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FitnessTracker.Application.WorkoutOrdering
{
    public interface IWorkoutOrderingApllier
    {
        IQueryable<Workout> ApplyOrdering(IQueryable<Workout> query, WorkoutOrderingDTO ordering);
    }
    internal class WorkoutOrderingApllier
        : IWorkoutOrderingApllier
    {
        private readonly Dictionary<WorkoutOrderingType, IWorkoutOrder> _orders;

        public WorkoutOrderingApllier(IEnumerable<IWorkoutOrder> orders)
        {
            _orders = orders.ToDictionary(x => x.WorkoutOrderingType);
        }
        public IQueryable<Workout> ApplyOrdering(IQueryable<Workout> query,
            WorkoutOrderingDTO ordering)
        {
            try
            {
                if (ordering.OrderBy is null)
                {
                    return query;
                }
                var parameter = Expression.Parameter(typeof(Workout), "workout");

                WorkoutOrderingType orderingType = Enum.Parse<WorkoutOrderingType>(ordering.OrderBy!, true);

                if(!_orders.TryGetValue(orderingType, out var order))
                {
                    throw new NotImplementedFunctionalityException($"No order implementation found for order type: {orderingType}");
                }

                var property = order.BuildExpression(parameter);

                var selector = Expression.Lambda(property, parameter);

                bool desc = ordering.Descending ?? false;

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

                return query.Provider.CreateQuery<Workout>(call);
            }
            catch (FormatException)
            {
                throw new WrongWorkoutOrderingTypeFormatException($"No ordering field: {ordering.OrderBy}");
            }
        }
    }
}
