using FitnessTracker.Shared.DTO.Application.Workout;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Application.Interfaces.DataSellection.Ordering
{
    public interface IWorkoutOrderingApplier
    {
        IQueryable<Workout> ApplyOrdering(IQueryable<Workout> query,
            WorkoutOrderingDTO ordering);
    }
}
