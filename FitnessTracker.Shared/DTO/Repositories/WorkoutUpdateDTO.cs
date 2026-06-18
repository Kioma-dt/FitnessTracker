using FitnessTracker.Shared.Enums;
namespace FitnessTracker.Shared.DTO.Repositories
{
    public record WorkoutUpdateDTO(string? Title = null,
                                   WorkoutType? Type = null,
                                   TimeSpan? Duration = null,
                                   int? CaloriesBurned = null,
                                   DateTime? WorkoutDate = null,
                                   List<ExerciseUpdateDTO>? Exercises = null,
                                   List<string>? ProgressPhotos = null);

    public record ExerciseUpdateDTO(string Name, List<SetUpdateDTO> Sets);

    public record SetUpdateDTO(double Weight, int Reps);
}
