namespace FitnessTracker.Shared.DTO.Application.Workout
{
    public record WorkoutFilterDTO
    (
        WorkoutFilterType FilterType,
        string FilterValue
    );
}
