using FitnessTracker.Shared.DTO.Responses;
using FitnessTracker.Shared.Enums;
using FitnessTracker.Shared.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitnessTracker.Shared.DTO.Requests
{
    public record WorkoutCreateRequestDTO
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
    )
    {
        private string? _userId;
        public void SetUserId(string userId)
        {
            _userId = userId;
        }
        public string GetUserId()
        {
            if (string.IsNullOrEmpty(_userId))
            {
                throw new InvalidOperationException("UserId is not set. Please call SetUserId before accessing UserId.");
            }
            return _userId;
        }
    }
}
