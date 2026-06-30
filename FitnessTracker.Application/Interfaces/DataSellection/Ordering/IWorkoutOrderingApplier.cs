using FitnessTracker.Shared.DTO.Application.Workout;

namespace FitnessTracker.Application.Interfaces.DataSellection.Ordering
{
    public interface IWorkoutOrderingApplier
    {
        IQueryable<Workout> ApplyOrdering(
            IQueryable<Workout> query,
            WorkoutOrderingDTO ordering);
    }
}
