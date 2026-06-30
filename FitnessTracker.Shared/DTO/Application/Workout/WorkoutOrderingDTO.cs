namespace FitnessTracker.Shared.DTO.Application.Workout
{
    public record WorkoutOrderingDTO
    (
        WorkoutOrderingType OrderBy,
        bool Descending
    );
}
