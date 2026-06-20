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
    {
        private string? _userId;

        public WorkoutCreateRequestDTO(string title,
            WorkoutType type,
            int durationInMinutes,
            int caloriesBurned,
            DateTime workoutDate,
            List<ExerciseCreateRequestDTO> exercises,
            List<string> progressPhotos)
        {
            Title = title;
            Type = type;
            DurationInMinutes = durationInMinutes;
            CaloriesBurned = caloriesBurned;
            WorkoutDate = workoutDate;
            Exercises = exercises;
            ProgressPhotos = progressPhotos;
        }

        [Required]
        [StringLength(128, MinimumLength = 3)]
        public string Title { get; set; }

        public WorkoutType Type { get; set; }

        [Range(1, 1440)]                                       //Maximum minutes in 24 hours
        public int DurationInMinutes { get; set; }

        [Range(0, 6000)]                                       // Maximum callories burned by human a day
        public int CaloriesBurned { get; set; }

        [NotFutureDate(
            ErrorMessage = "WorkoutDate should not be in future")]
        public DateTime WorkoutDate { get; set; }

        [Required]
        [MinLength(1, 
            ErrorMessage = "Workout must contain at leats 1 Exercise")]
        [MaxLength(1000)]
        public List<ExerciseCreateRequestDTO> Exercises { get; set; }

        [MaxLength(1000)]
        public List<string> ProgressPhotos { get; set; }

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
