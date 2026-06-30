namespace FitnessTracker.Shared.DTO.Queries.Workout
{
    public record WorkoutOrderingQueryDTO
    {
        public WorkoutOrderingType? OrderBy { get; set; }
        public bool? Descending { get; set; }
    }
}
