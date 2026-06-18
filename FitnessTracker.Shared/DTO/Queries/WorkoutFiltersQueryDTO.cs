using FitnessTracker.Shared.Enums;

namespace FitnessTracker.Shared.DTO.Queries
{
    public record WorkoutFiltersQueryDTO
        (
            string? FromDate,
            string? ToDate,
            string? MinDuration,
            string? MaxDuration,
            string? WorkoutType
        )
    {
        public List<WorkoutFilterDTO> ToList()
        {
            return new List<WorkoutFilterDTO>
            {
                new WorkoutFilterDTO(WorkoutFilterType.FromDate, FromDate),
                new WorkoutFilterDTO(WorkoutFilterType.ToDate, ToDate),
                new WorkoutFilterDTO(WorkoutFilterType.MinDuration, MinDuration),
                new WorkoutFilterDTO(WorkoutFilterType.MaxDuration, MaxDuration),
                new WorkoutFilterDTO(WorkoutFilterType.WorkoutType, WorkoutType)
            };
        }
    }
}
