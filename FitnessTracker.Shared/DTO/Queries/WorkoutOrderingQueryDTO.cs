namespace FitnessTracker.Shared.DTO.Queries
{
    public record WorkoutOrderingQueryDTO
        (
            string? OrderBy,
            bool? Descending
        );
}
