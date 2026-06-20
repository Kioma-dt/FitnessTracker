using FitnessTracker.Shared.Enums;
using FitnessTracker.Shared.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitnessTracker.Shared.DTO.Requests
{
    public record WorkoutPatchRequestDTO
    (
        [property: StringLength(128, MinimumLength = 3)]
        string? Title,

        WorkoutType? Type,

        [property: Range(1, 1440)]            //Maximum minutes in 24 hours
        int? DurationInMinutes,

        [property: Range(0, 6000)]            // Maximum callories burned by human a day
        int? CaloriesBurned,

        [property: NotFutureDate(
            ErrorMessage = "WorkoutDate should not be in future")]
        DateTime? WorkoutDate,

        [property: MinLength(1, ErrorMessage = "Workout should have at least one exercise")]
        [property: MaxLength(1000)]
        List<ExerciseCreateRequestDTO>? Exercises,

        [property: MaxLength(1000)]
        List<string>? ProgressPhotos
    );
}
