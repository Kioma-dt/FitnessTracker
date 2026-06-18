using FitnessTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Shared.DTO
{
    public record WorkoutFiltersDTO
    (
        DateTime? FromDate,
        DateTime? ToDate,
        TimeSpan? MinDuration,
        TimeSpan? MaxDuration,
        WorkoutType? Type
    );
}
