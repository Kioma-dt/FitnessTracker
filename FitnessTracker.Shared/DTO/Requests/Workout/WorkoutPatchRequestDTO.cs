using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Shared.DTO.Requests.Workout
{
    public record WorkoutPatchRequestDTO
    {
        public WorkoutPatchRequestDTO(
            string? title, 
            WorkoutType? type,
            int? durationInMinutes,
            int? caloriesBurned,
            DateTime? workoutDate,
            List<ExerciseCreateRequestDTO>? exercises,
            List<string>? progressPhotos)
        {
            Title = title;
            Type = type;
            DurationInMinutes = durationInMinutes;
            CaloriesBurned = caloriesBurned;
            WorkoutDate = workoutDate;
            Exercises = exercises;
            ProgressPhotos = progressPhotos;
        }

        [StringLength(128, MinimumLength = 3)]
        public string? Title { get; set; }

        public WorkoutType? Type { get; set; }

        [Range(1, 1440)]                                                            //Maximum minutes in 24 hours
        public int? DurationInMinutes { get; set; }

        [Range(0, 6000)]                                                            // Maximum callories burned by human a day
        public int? CaloriesBurned { get; set; }

        [NotFutureDate(ErrorMessage = "WorkoutDate should not be in future")]
        [UtcDate(ErrorMessage = "WorkoutDate should be in UTC format")]
        public DateTime? WorkoutDate { get; set; }

        [MinLength(1, ErrorMessage = "Workout should have at least one exercise")]
        [MaxLength(1000)]
        public List<ExerciseCreateRequestDTO>? Exercises { get; set; }

        [MaxLength(1000)]
        [UrlList(ErrorMessage = "ProgressPhotos should be a list of valid URLs")]
        public List<string>? ProgressPhotos { get; set; }
    }
}
