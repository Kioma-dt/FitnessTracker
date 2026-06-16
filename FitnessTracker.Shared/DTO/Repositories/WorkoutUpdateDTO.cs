using FitnessTracker.Shared.Enums;
namespace FitnessTracker.Shared.DTO.Repositories
{
    public record WorkoutUpdateDTO(string Title,
                                   WorkoutType Type,
                                   TimeSpan Duration,
                                   int CaloriesBurned,
                                   DateTime WorkoutDate);
}
