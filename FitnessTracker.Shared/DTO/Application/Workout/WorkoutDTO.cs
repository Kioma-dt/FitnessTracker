namespace FitnessTracker.Shared.DTO.Application.Workout
{
    public record WorkoutDTO
    (
        string Title,
        WorkoutType Type,
        string UserId,
        TimeSpan Duration,
        int CaloriesBurned,
        DateTime WorkoutDate,
        List<ExerciseDTO> Exercises,
        List<string> ProgressPhotos,
        string Id
    );

    public record ExerciseDTO
    (
        string Name,
        List<SetDTO> Sets
    );

    public record SetDTO
    (
        double Weight,
        int Reps
    );
}
