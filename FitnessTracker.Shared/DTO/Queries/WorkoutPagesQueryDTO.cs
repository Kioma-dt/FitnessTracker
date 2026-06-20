using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitnessTracker.Shared.DTO.Queries
{
    public record WorkoutPagesQueryDTO
    (
        [property: DefaultValue(1)]
        [property: Range(1, int.MaxValue, ErrorMessage = "Page must be greater than or equal to 1")]
        int Page = 1,

        [property: DefaultValue(10)]
        [property: Range(1, int.MaxValue, ErrorMessage = "PageSize must be greater than or equal to 1")]
        int PageSize = 10
    );
}
