using FitnessTracker.Shared.DTO.Responses;
using FitnessTracker.Shared.Enums;

namespace FitnessTracker.Shared.DTO
{
    public record UpdatedWorkoutDTO
    (
        bool IsUpdated,
        WorkoutDTO Workout
    );
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
        List<SetResponseDTO> Sets
    );

    public record SetDTO
    (
        double Weight,
        int Reps
    );
}
