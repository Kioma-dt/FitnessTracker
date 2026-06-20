using FitnessTracker.Shared.Enums;
using FitnessTracker.Shared.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Shared.DTO.Requests
{
    public record WorkoutUpdateRequestDTO
    (

        [property: Required]
        [property: StringLength(128, MinimumLength = 3)]
        string Title,

        WorkoutType Type,

        [property: Range(1, 1440)]                                       //Maximum minutes in 24 hours
        int DurationInMinutes,

        [property: Range(0, 6000)]                                       // Maximum callories burned by human a day
        int CaloriesBurned,

        [NotFutureDate(
            ErrorMessage = "WorkoutDate should not be in future")]
        DateTime WorkoutDate,

        [property: Required]
        [property: MinLength(1,
            ErrorMessage = "Workout must contain at leats 1 Exercise")]
        [property: MaxLength(1000)]
        List<ExerciseCreateRequestDTO> Exercises,

        [property: MaxLength(1000)]
        List<string> ProgressPhotos
    );
}
