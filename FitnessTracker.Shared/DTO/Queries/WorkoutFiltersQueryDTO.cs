using FitnessTracker.Shared.Enums;

namespace FitnessTracker.Shared.DTO.Queries
{
    public record WorkoutFiltersQueryDTO
        (
            DateTime? FromDate,
            DateTime? ToDate,
            TimeSpan? MinDuration,
            TimeSpan? MaxDuration,
            WorkoutType? WorkoutType
        )
    {
        public List<WorkoutFilterDTO> ToList()
        {
            return new List<WorkoutFilterDTO>
            {
                new WorkoutFilterDTO(WorkoutFilterType.FromDate, FromDate?.ToUniversalTime().ToString()),
                new WorkoutFilterDTO(WorkoutFilterType.ToDate, ToDate?.ToUniversalTime().ToString()),
                new WorkoutFilterDTO(WorkoutFilterType.MinDuration, MinDuration.ToString()),
                new WorkoutFilterDTO(WorkoutFilterType.MaxDuration, MaxDuration.ToString()),
                new WorkoutFilterDTO(WorkoutFilterType.WorkoutType, WorkoutType.ToString())
            };
        }
    }
}
