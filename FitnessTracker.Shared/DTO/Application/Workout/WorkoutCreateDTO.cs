namespace FitnessTracker.Shared.DTO.Application.Workout
{
    public record WorkoutCreateDTO
    (
        string? Id,
        string Title,
        WorkoutType Type,
        string? UserId,
        TimeSpan Duration,
        int CaloriesBurned,
        DateTime WorkoutDate,
        List<ExerciseCreateDTO> Exercises,
        List<string> ProgressPhotos
    );
}
