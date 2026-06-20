using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitnessTracker.Shared.DTO.Queries
{
    public record WorkoutPagesQueryDTO
    (
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than or equal to 1")]
        int Page = 1,

        [Range(1, int.MaxValue, ErrorMessage = "PageSize must be greater than or equal to 1")]
        int PageSize = 10
    );
}
