using FitnessTracker.Shared.Enums;

namespace FitnessTracker.Shared.DTO
{
    public record WorkoutCreateDTO
    (
        string Title,
        WorkoutType Type,
        string? UserId,
        TimeSpan Duration,
        int CaloriesBurned,
        DateTime WorkoutDate,
        List<ExerciseCreateDTO> Exercises,
        List<string> ProgressPhotos
    );


    public record ExerciseCreateDTO
    (
        string Name,
        List<SetCreateDTO> Sets
    );

    public record SetCreateDTO
    (
        double Weight,
        int Reps
    );
}
