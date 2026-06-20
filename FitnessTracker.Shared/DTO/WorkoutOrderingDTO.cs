using FitnessTracker.Shared.Enums;

namespace FitnessTracker.Shared.DTO
{
    public record WorkoutOrderingDTO
    (
        WorkoutOrderingType OrderBy,
        bool Descending
    );
}
