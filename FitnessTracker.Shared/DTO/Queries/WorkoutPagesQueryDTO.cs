using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Shared.DTO.Queries
{
    public record WorkoutPagesQueryDTO
    (
        int Page = 1,
        int PageSize = 10
    );
}
