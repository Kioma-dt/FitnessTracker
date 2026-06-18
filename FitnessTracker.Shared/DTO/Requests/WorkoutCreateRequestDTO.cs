using FitnessTracker.Shared.DTO.Responses;
using FitnessTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitnessTracker.Shared.DTO.Requests
{
    public record WorkoutCreateRequestDTO
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

        List<ExerciseCreateRequestDTO> Exercises,

        List<string> ProgressPhotos 
    );

    public record ExerciseCreateRequestDTO
    (
        [Required]
        [StringLength(128, MinimumLength = 3)]
        string Name,

        List<SetCreateRequestDTO> Sets
    );

    public record SetCreateRequestDTO
    (
        [Required]
        [Range(0, 2500d)]       // World record for weight is 2422 kg
        double Weight,

        [Required]
        [Range(1, 12000)]       // World record for push ups in a row 10 507 
        int Reps
    );
}
