using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitnessTracker.Shared.DTO.Queries
{
    public record WorkoutPagesQueryDTO
    {
        [DefaultValue(1)]
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than or equal to 1")]
        public int Page { get; set; } = 1;

        [DefaultValue(10)]
        [Range(1, int.MaxValue, ErrorMessage = "PageSize must be greater than or equal to 1")]
        public int PageSize { get; set; } = 10;
    };
}
