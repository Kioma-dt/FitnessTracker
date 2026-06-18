using FitnessTracker.Shared.Enums;
using System.ComponentModel.DataAnnotations;

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
        DateTime WorkoutDate,

        List<ExerciseUpdateRequestDTO> Exercises,

        List<string> ProgressPhotos
    );

    public record ExerciseUpdateRequestDTO
    (
        [Required]
        [StringLength(128, MinimumLength = 3)]
        string Name,

        List<SetUpdateRequestDTO> Sets
    );

    public record SetUpdateRequestDTO
    (
        [Required]
        [Range(0, 2500d)]       // World record for weight is 2422 kg
        double Weight,

        [Required]
        [Range(1, 12000)]       // World record for push ups in a row 10 507 
        int Reps
    );
}
