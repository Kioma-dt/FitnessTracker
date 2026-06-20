using FitnessTracker.Shared.Enums;

namespace FitnessTracker.Shared.DTO.Queries
{
    public record WorkoutOrderingQueryDTO
        (
            WorkoutOrderingType? OrderBy,
            bool? Descending
        );
}
