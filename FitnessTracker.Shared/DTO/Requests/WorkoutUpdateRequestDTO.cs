using FitnessTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitnessTracker.Shared.DTO.Requests
{
    public record WorkoutUpdateRequestDTO
    (
        [Required]
        [StringLength(128, MinimumLength = 3)]
        string Title,

        [Required]
        WorkoutType Type,

        [Required]
        [Range(1, 1440)]            //Maximum minutes in 24 hours
        int DurationInMinutes,

        [Required]
        [Range(1, 6000)]            // Maximum callories burned by human a day
        int CaloriesBurned,

        [Required]
        [DataType(DataType.Date)]
        DateTime WorkoutDate
    );
}
