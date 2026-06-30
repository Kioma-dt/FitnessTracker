using FitnessTracker.Application.Interfaces.DataSellection.Ordering;
using FitnessTracker.Shared.DTO.Application.Workout;
using FitnessTracker.Shared.Exceptions.InternalServerError;

using System.Linq.Expressions;

namespace FitnessTracker.Inrastructure.DataSellection.Ordering.WorkoutOrders
{
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
