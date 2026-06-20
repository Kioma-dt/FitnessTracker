using FitnessTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Shared.DTO
{
    public record WorkoutFilterDTO
    (
        WorkoutFilterType FilterType,
        string FilterValue
    );
}
